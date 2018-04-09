using Authsome.Models;
using Authsome.Portable.Builder;
using Authsome.Portable.Extentions;
using Authsome.Portable.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authsome
{
    public interface IAuthsomeService
    {
        Provider Provider { get; set; }
        OAuth oAuth { get; set; }

        void InitGlobalRefreshToken(Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null);

        Task<HttpResponseWrapper<T>> GetAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null);

        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body, MediaType mediaType, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null);

        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body, MediaType mediaType, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null);

        Task<HttpResponseWrapper<T>> DeleteAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null);

        //string RequestAuthorization();
        //Task<TokenResponse> RequestBearerTokenAsync(string code);
        //Task<HttpResponseWrapper<TokenResponse>> RefreshTheAccessTokenAsync(string refreshToken);
        //Task<bool> RevokeTokenAsync(TokenType tokenType, string token);

        //Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, object body = null, string userAgent = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null);
    }

    public class AuthsomeService : IAuthsomeService
    {
        private Action<HttpResponseWrapper<TokenResponse>> RefreshedToken;

        public OAuth oAuth { get; set; }
        public AuthsomeService()
        {
            oAuth = new OAuth();
        }

        public Provider Provider
        {
            get
            {
                return oAuth.Provider;
            }
            set
            {
                oAuth.Provider = value;
            }
        }

        public void InitGlobalRefreshToken(Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null)
        {
            this.RefreshedToken = RefreshedToken;
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Get, url, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            HttpContent bodyContent = null;
            if (body != null)
            {
                bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType);
            }
            return await factory.Request<T>(HttpOption.Post, url, bodyContent, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body, MediaType mediaType, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            HttpContent bodyContent = null;
            if (body != null)
            {
                bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType.GetMediaType());
            }
            return await factory.Request<T>(HttpOption.Post, url, bodyContent, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Post, url, content, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Post, url, content, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Put, url, content, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            HttpContent bodyContent = null;
            if (body != null)
            {
                bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType);
            }
            return await factory.Request<T>(HttpOption.Put, url, bodyContent, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body, MediaType mediaType, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            HttpContent bodyContent = null;
            if (body != null)
            {
                bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType.GetMediaType());
            }
            return await factory.Request<T>(HttpOption.Put, url, bodyContent, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Put, url, content, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }

        public async Task<HttpResponseWrapper<T>> DeleteAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null)
        {
            var factory = new RequestFactory();
            return await factory.Request<T>(HttpOption.Delete, url, oAuth: oAuth, HeaderBuilder: HeaderBuilder, RefreshedToken: RefreshedToken);
        }
    }
}