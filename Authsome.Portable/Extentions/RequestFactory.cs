using Authsome.Models;
using Authsome.Portable.Builder;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Authsome.Portable.Extentions
{
    public class RequestFactory
    {
        private HttpContent httpContent = null;
        private int attemptsCount = 0;
        public async Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, HttpContent bodyContent = null, OAuth oAuth = null, Action<IHeaderRequest> HeaderBuilder = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null, bool isClone = false)
        {
            if (!isClone)
            {
                httpContent = await bodyContent.CloneAsync();
            }
            else
            {
                if (httpContent != null)
                {
                    bodyContent = httpContent;
                }
            }

            var client = new HttpClient();
            SetDefaultConfigs(client);
            HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

            if (oAuth != null && oAuth.Provider != null && !String.IsNullOrWhiteSpace(oAuth.Provider.APIBaseUrl))
            {
                client.BaseAddress = new Uri(oAuth.Provider.APIBaseUrl);
            }

            if (oAuth != null && oAuth.Provider != null && oAuth.Provider.TokenResponse != null && !String.IsNullOrWhiteSpace(oAuth.Provider.TokenResponse.access_token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oAuth.Provider.TokenResponse.access_token);
            }

            HttpResponseMessage httpResponseMessage = null;
            switch (method)
            {
                case HttpOption.Post:
                    httpResponseMessage = await client.PostAsync(new Uri(url, UriKind.RelativeOrAbsolute), bodyContent);
                    break;
                case HttpOption.Get:
                    httpResponseMessage = await client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute));
                    break;
                case HttpOption.Put:
                    httpResponseMessage = await client.PutAsync(new Uri(url, UriKind.RelativeOrAbsolute), bodyContent);
                    break;
                case HttpOption.Delete:
                    httpResponseMessage = await client.DeleteAsync(new Uri(url, UriKind.RelativeOrAbsolute));
                    break;
            }

            var wrap = new HttpResponseWrapper<T>();
            if (httpResponseMessage != null)
            {
                wrap.httpStatusCode = httpResponseMessage.StatusCode;

                if (wrap.httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    wrap.ErrorJson = await httpResponseMessage.Content.ReadAsStringAsync();

                    // attempt to renew and recall the same api
                    if (oAuth != null && oAuth.Provider != null && oAuth.Provider.TokenResponse != null && !String.IsNullOrWhiteSpace(oAuth.Provider.TokenResponse.refresh_token))
                    {
                        var tokenReponse = await oAuth.RefreshTheAccessTokenAsync(oAuth.Provider.TokenResponse);

                        // regardless of the state of the token (valid or not) we want to notify what happened on the request
                        if (RefreshedToken != null)
                        {
                            RefreshedToken(tokenReponse);
                        }

                        if (tokenReponse.httpStatusCode == HttpStatusCode.OK)
                        {
                            // store the token in memory
                            oAuth.Provider.TokenResponse = tokenReponse.Content;

                            // since the token was refresh we can now re-attempted the actual call
                            attemptsCount++;

                            if (attemptsCount > 3)
                            {
                                return null;
                            }
                            return await Request<T>(method, url, bodyContent, oAuth, HeaderBuilder, RefreshedToken, isClone: true);
                        }
                    }
                }
                else if (wrap.httpStatusCode == HttpStatusCode.BadRequest)
                {
                    if (httpResponseMessage.Content != null)
                    {
                        wrap.ErrorJson = await httpResponseMessage.Content.ReadAsStringAsync();
                    }

                    if (httpContent != null)
                    {
                        httpContent.Dispose();
                        httpContent = null;
                    }
                }
                else if (wrap.httpStatusCode == HttpStatusCode.Forbidden)
                {
                    // you do not have permission to continue
                    if (httpResponseMessage.Content != null)
                    {
                        wrap.ErrorJson = await httpResponseMessage.Content.ReadAsStringAsync();
                    }

                    if (httpContent != null)
                    {
                        httpContent.Dispose();
                        httpContent = null;
                    }
                }
                else
                {
                    if (httpResponseMessage.Content != null)
                    {
                        wrap.Content = await httpResponseMessage.Content.ReadAsAsync<T>();
                    }

                    if (httpContent != null)
                    {
                        httpContent.Dispose();
                        httpContent = null;
                    }
                }
            }

            return wrap;
        }

        private void SetDefaultConfigs(HttpClient client)
        {
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
        }
    }
}