using Authsome.Models;
using Authsome.Portable.Builder;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authsome.Portable.Extentions
{
    public class RequestFactory
    {
        public async Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, HttpContent bodyContent = null, OAuth oAuth = null, Action<IHeaderRequest> HeaderBuilder = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null)
        {
            using (var client = new HttpClient())
            {
                //SetDefaultConfigs(client);
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));


                T obj = default(T);

                if (oAuth != null && oAuth.Provider != null && !String.IsNullOrWhiteSpace(oAuth.Provider.APIBaseUrl))
                {
                    client.BaseAddress = new Uri(oAuth.Provider.APIBaseUrl);
                }

                string accessToken = null;
                if (oAuth != null && oAuth.Provider != null && oAuth.Provider.TokenResponse != null && !String.IsNullOrWhiteSpace(oAuth.Provider.TokenResponse.access_token))
                {
                    accessToken = oAuth.Provider.TokenResponse.access_token;
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
                            var tokenReponse = await oAuth.RefreshTheAccessTokenAsync(oAuth.Provider.TokenResponse.refresh_token);

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
                                return await Request<T>(method, url, bodyContent, oAuth, HeaderBuilder, RefreshedToken);
                            }
                        }
                    }
                    else if (wrap.httpStatusCode == HttpStatusCode.BadRequest)
                    {
                        if (httpResponseMessage.Content != null)
                        {
                            wrap.ErrorJson = await httpResponseMessage.Content.ReadAsStringAsync();
                        }
                    }
                    else if (wrap.httpStatusCode == HttpStatusCode.Forbidden)
                    {
                        // you do not have permission to continue
                        if (httpResponseMessage.Content != null)
                        {
                            wrap.ErrorJson = await httpResponseMessage.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        if (httpResponseMessage.Content != null)
                        {
                            wrap.Content = await httpResponseMessage.Content.ReadAsAsync<T>();
                        }
                    }
                }

                return wrap;
            }
        }

        private void SetDefaultConfigs(HttpClient client)
        {
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
        }
    }
}