using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authsome
{
    public static class HttpClientExtention
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            //T obj = default(T);
            var readString = await httpContent.ReadAsStringAsync();
            if (!String.IsNullOrWhiteSpace(readString))
            {
                return JsonConvert.DeserializeObject<T>(readString);
            }
            return default(T);
        }

        internal static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null)
                return null;

            Stream stream = new MemoryStream();
            await content.CopyToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;

            StreamContent clone = new StreamContent(stream);
            foreach (var header in content.Headers)
                clone.Headers.Add(header.Key, header.Value);

            return clone;
        }
    }
}