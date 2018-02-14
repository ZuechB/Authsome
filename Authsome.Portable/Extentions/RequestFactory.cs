using Authsome.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authsome.Portable.Extentions
{
    public class RequestFactory
    {
        public async Task<HttpResponseWrapper<T>> Request<T>(HttpClient client, HttpOption method, string url, HttpContent bodyContent = null, Provider Provider = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null)
        {
            T obj = default(T);

            if (Provider != null && !String.IsNullOrWhiteSpace(Provider.APIBaseUrl))
            {
                client.BaseAddress = new Uri(Provider.APIBaseUrl);
            }

            string accessToken = null;
            if (Provider != null && Provider.TokenResponse != null && !String.IsNullOrWhiteSpace(Provider.TokenResponse.access_token))
            {
                accessToken = Provider.TokenResponse.access_token;
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
                    // attempt to renew and recall the same api
                    if (Provider != null && Provider.TokenResponse != null && !String.IsNullOrWhiteSpace(Provider.TokenResponse.refresh_token))
                    {
                        var oAuth = new OAuth(Provider);
                        var tokenReponse = await oAuth.RefreshTheAccessTokenAsync(Provider.TokenResponse.refresh_token);

                        // regardless of the state of the token (valid or not) we want to notify what happened on the request
                        if (RefreshedToken != null)
                        {
                            RefreshedToken(tokenReponse);
                        }

                        if (tokenReponse.httpStatusCode == HttpStatusCode.OK)
                        {
                            // store the token in memory
                            Provider.TokenResponse = tokenReponse.Content;

                            // since the token was refresh we can now re-attempted the actual call
                            return await Request<T>(client, method, url, bodyContent);
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
}