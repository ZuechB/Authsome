using System.Net;

namespace Authsome.Models
{
    public class HttpResponseWrapper<T>
    {
        public T Content { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
        public string ErrorJson { get; set; }
    }
}