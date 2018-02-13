using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authsome
{
    public static class HttpClientExtention
    {
        public static void SetBasicAuthentication(this HttpClient client, string username, string password)
        {
            var byteArray = Encoding.UTF8.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private static void SetDefaultConfigs(this HttpClient client, string userAgent, string accessToken)
        {
            if (!String.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (!String.IsNullOrWhiteSpace(userAgent))
            {
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }

            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string url, object body, string userAgent = null, string token = null)
        {
            client.SetDefaultConfigs(userAgent, token);

            StringContent content = null;
            if (body != null)
                content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            else
                content = new StringContent("", Encoding.UTF8, "application/json");

            return await client.PostAsync(new Uri(url, UriKind.RelativeOrAbsolute), content);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient client, string url, object body, string userAgent = null, string token = null)
        {
            client.SetDefaultConfigs(userAgent, token);

            StringContent content = null;
            if (body != null)
                content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            else
                content = new StringContent("", Encoding.UTF8, "application/json");

            return await client.PostAsync(new Uri(url, UriKind.RelativeOrAbsolute), content);
        }

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient client, string url, string userAgent = null, string token = null)
        {
            client.SetDefaultConfigs(userAgent, token);
            return await client.DeleteAsync(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public static async Task<HttpResponseMessage> GetAsJsonAsync(this HttpClient client, string url, string userAgent = null, string token = null)
        {
            client.SetDefaultConfigs(userAgent, token);
            return await client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute));
        }

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
                catch(Exception exp)
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}