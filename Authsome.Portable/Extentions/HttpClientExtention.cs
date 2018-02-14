using Newtonsoft.Json;
using System;
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
                try
                {
                    return JsonConvert.DeserializeObject<T>(readString);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}