using Authsome.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authsome
{
    public interface IAuthsomeService
    {
        Provider Provider { get; set; }


        string RequestAuthorization();
        Task<TokenResponse> RequestBearerTokenAsync(string code);
        Task<HttpResponseWrapper<TokenResponse>> RefreshTheAccessTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(TokenType tokenType, string token);


        Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, object body = null, string userAgent = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null);
    }

    public delegate void TokenRenewalDelegate(TokenResponse tokenResponse);

    public class AuthsomeService : IAuthsomeService
    {
        public AuthsomeService() { }

        public Provider Provider { get; set; }

        public string RequestAuthorization()
        {
            if (Provider != null)
            {
                var scope = "";
                for (int i = 0; i < Provider.scope.Length; i++)
                {
                    if (i > 0) { scope += "%20"; }
                    scope += Provider.scope[i];
                }

                return Provider.authorizationUrl
                    .Replace("{clientId}", Provider.clientId)
                    .Replace("{redirectUrl}", Provider.redirectUrl.Replace("{provider}", ((int)Provider.Id).ToString()))
                    .Replace("{state}", Provider.state)
                    .Replace("{scope}", scope)
                    .Replace("{response_type}", Provider.response_type);
            }
            return "";
        }

        public async Task<TokenResponse> RequestBearerTokenAsync(string code)
        {
            if (Provider != null)
            {
                var client = new HttpClient();
                client.SetBasicAuthentication(Provider.clientId, Provider.secret);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", Provider.redirectUrl.Replace("{provider}", ((int)Provider.Id).ToString())),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                });
                var response = await client.PostAsync(Provider.TokenBearerUrl, content);

                Provider.TokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
                return Provider.TokenResponse;
            }
            return null;
        }

        public async Task<HttpResponseWrapper<TokenResponse>> RefreshTheAccessTokenAsync(string refreshToken)
        {
            if (Provider != null)
            {
                var httpResponseWrapper = new HttpResponseWrapper<TokenResponse>();

                var client = new HttpClient();
                client.SetBasicAuthentication(Provider.clientId, Provider.secret);

                var response = await client.PostAsync(Provider.RefreshAccessTokenUrl,
                    new StringContent("grant_type=refresh_token&refresh_token=" + refreshToken, Encoding.UTF8, "application/x-www-form-urlencoded"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    httpResponseWrapper.Content = await response.Content.ReadAsAsync<TokenResponse>();
                }
                else
                {
                    httpResponseWrapper.ErrorJson = await response.Content.ReadAsStringAsync();
                }

                httpResponseWrapper.httpStatusCode = response.StatusCode;

                return httpResponseWrapper;
            }
            return null;
        }

        public async Task<bool> RevokeTokenAsync(TokenType tokenType, string token)
        {
            if (Provider != null)
            {
                var tokenHintType = "refresh_token";
                if (tokenType == TokenType.AccessToken)
                {
                    tokenHintType = "access_token";
                }

                var client = new HttpClient();
                client.SetBasicAuthentication(Provider.clientId, Provider.secret);
                var response = await client.PostAsJsonAsync(Provider.RevokeUrl, new
                {
                    token_type_hint = tokenHintType,
                    token = token
                });

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, object body = null, string userAgent = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null)
        {
            T obj = default(T);
            using (HttpClient client = new HttpClient())
            {
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
                switch(method)
                {
                    case HttpOption.Post:
                        httpResponseMessage = await client.PostAsJsonAsync(url, body, userAgent, accessToken);
                        break;
                    case HttpOption.Get:
                        httpResponseMessage = await client.GetAsJsonAsync(url, userAgent, accessToken);
                        break;
                    case HttpOption.Put:
                        httpResponseMessage = await client.PutAsJsonAsync(url, body, userAgent, accessToken);
                        break;
                    case HttpOption.Delete:
                        httpResponseMessage = await client.DeleteAsJsonAsync(url, userAgent, accessToken);
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
                            var tokenReponse = await RefreshTheAccessTokenAsync(Provider.TokenResponse.refresh_token);

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
                                return await Request<T>(method, url, body);
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
}