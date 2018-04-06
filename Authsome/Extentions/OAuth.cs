using Authsome.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authsome.Portable.Extentions
{
    public class OAuth
    {
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
                var authsome = new AuthsomeService();
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", Provider.redirectUrl.Replace("{provider}", ((int)Provider.Id).ToString())),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                });
                var response = await authsome.PostAsync<TokenResponse>(Provider.TokenBearerUrl, content, (headerRequest) =>
                {
                    headerRequest.IncludeBasicAuth(Provider.clientId, Provider.secret);
                });
                if (response.httpStatusCode == HttpStatusCode.OK)
                {
                    Provider.TokenResponse = response.Content;
                }

                return Provider.TokenResponse;
            }
            return null;
        }

        public async Task<HttpResponseWrapper<TokenResponse>> RefreshTheAccessTokenAsync(string refreshToken)
        {
            if (Provider != null)
            {
                var httpResponseWrapper = new HttpResponseWrapper<TokenResponse>();

                var authsome = new AuthsomeService();
                var response = await authsome.PostAsync<TokenResponse>(Provider.RefreshAccessTokenUrl, 
                    new StringContent("grant_type=refresh_token&refresh_token=" + refreshToken, Encoding.UTF8, "application/x-www-form-urlencoded"),
                    (header) =>
                    {
                        header.IncludeBasicAuth(Provider.clientId, Provider.secret);
                    });

                
                if (response.httpStatusCode == HttpStatusCode.OK)
                {
                    httpResponseWrapper.Content = response.Content;
                }
                else
                {
                    httpResponseWrapper.ErrorJson = response.ErrorJson;
                }

                httpResponseWrapper.httpStatusCode = response.httpStatusCode;

                return httpResponseWrapper;
            }
            return null;
        }

        //public async Task<bool> RevokeTokenAsync(TokenType tokenType, string token)
        //{
        //    if (Provider != null)
        //    {
        //        var tokenHintType = "refresh_token";
        //        if (tokenType == TokenType.AccessToken)
        //        {
        //            tokenHintType = "access_token";
        //        }

        //        var client = new HttpClient();
        //        client.SetBasicAuthentication(Provider.clientId, Provider.secret);
        //        var response = await client.PostAsJsonAsync(Provider.RevokeUrl, new
        //        {
        //            token_type_hint = tokenHintType,
        //            token = token
        //        });

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}