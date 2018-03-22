using Authsome.Models;
using Authsome.Portable.Builder;
using Authsome.Portable.Extentions;
using Authsome.Portable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authsome
{
    public interface IAuthsomeService
    {
        Provider Provider { get; set; }

        Task<HttpResponseWrapper<T>> GetAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, MediaType mediaType = MediaType.application_json, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PostAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body = null, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null);
        Task<HttpResponseWrapper<T>> DeleteAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null);

        //string RequestAuthorization();
        //Task<TokenResponse> RequestBearerTokenAsync(string code);
        //Task<HttpResponseWrapper<TokenResponse>> RefreshTheAccessTokenAsync(string refreshToken);
        //Task<bool> RevokeTokenAsync(TokenType tokenType, string token);

        //Task<HttpResponseWrapper<T>> Request<T>(HttpOption method, string url, object body = null, string userAgent = null, Action<HttpResponseWrapper<TokenResponse>> RefreshedToken = null);
    }

    public class AuthsomeService : IAuthsomeService
    {
        public Provider Provider { get; set; } // currently this must be defined by the user

        public AuthsomeService() { }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Get, url, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType);
                }

                return await factory.Request<T>(client, HttpOption.Post, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, MediaType mediaType = MediaType.application_json, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType.GetMediaType());
                }

                return await factory.Request<T>(client, HttpOption.Post, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, object body = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                }

                return await factory.Request<T>(client, HttpOption.Post, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Post, url, content, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PostAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Post, url, content, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, FormUrlEncodedContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Put, url, content, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                }

                return await factory.Request<T>(client, HttpOption.Put, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body = null, string mediaType = "application/json", Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType);
                }

                return await factory.Request<T>(client, HttpOption.Put, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, object body = null, MediaType mediaType = MediaType.application_json, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));

                HttpContent bodyContent = null;
                if (body != null)
                {
                    bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType.GetMediaType());
                }

                return await factory.Request<T>(client, HttpOption.Put, url, bodyContent, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> PutAsync<T>(string url, StringContent content = null, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Put, url, content, Provider: Provider);
            }
        }

        public async Task<HttpResponseWrapper<T>> DeleteAsync<T>(string url, Action<IHeaderRequest> HeaderBuilder = null)
        {
            using (var client = new HttpClient())
            {
                SetDefaultConfigs(client);
                var factory = new RequestFactory();
                HeaderBuilder?.Invoke(new HeaderRequest(client.DefaultRequestHeaders));
                return await factory.Request<T>(client, HttpOption.Delete, url, Provider: Provider);
            }
        }


        /// <summary>
        /// This fires the default settings for our httpclient before the user changes anything
        /// </summary>
        /// <param name="client"></param>
        private void SetDefaultConfigs(HttpClient client)
        {
            //client.DefaultRequestHeaders
            //    .Accept
            //    .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
        }
    }
}