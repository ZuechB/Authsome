using System;
using System.Net.Http.Headers;
using System.Text;

namespace Authsome.Portable.Builder
{
    public interface IHeaderRequest
    {
        IHeaderRequest IncludeHeader(string key, string value);
        IHeaderRequest IncludeUserAgent(string value);
        //IHeaderRequest IncludeAcceptMediaType(string mediaType);
        IHeaderRequest IncludeBearerAuthentication(string token);
        IHeaderRequest IncludeBasicAuth(string username, string password);
    }

    public class HeaderRequest : IHeaderRequest
    {
        public HttpRequestHeaders requestMessage { get; }

        public HeaderRequest(HttpRequestHeaders httpRequestHeaders)
        {
            requestMessage = httpRequestHeaders;
        }

        /// <summary>
        /// Uses a Bearer token to authorize your request
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IHeaderRequest IncludeBearerAuthentication(string token)
        {
            requestMessage.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return this;
        }

        /// <summary>
        /// Enables Basic Authorization for your request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IHeaderRequest IncludeBasicAuth(string username, string password)
        {
            var byteArray = Encoding.UTF8.GetBytes(username + ":" + password);
            requestMessage.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return this;
        }

        /// <summary>
        /// Add a custom header to your request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHeaderRequest IncludeHeader(string name, string value)
        {
            requestMessage.Add(name, value);
            return this;
        }

        /// <summary>
        /// Assign the user agent in the header
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHeaderRequest IncludeUserAgent(string value)
        {
            IncludeHeader("User-Agent", value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaType">Defaults to "application/json"</param>
        /// <returns></returns>
        //public IHeaderRequest IncludeAcceptMediaType(string mediaType)
        //{
        //    requestMessage
        //        .Accept
        //        .Add(new MediaTypeWithQualityHeaderValue(mediaType)); //ACCEPT header
        //    return this;
        //}
    }
}