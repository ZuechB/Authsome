using Amplitude.Models;
using Authsome;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amplitude
{
    class Program
    {
        static IAuthsomeService authsomeService;
        const string apiKey = "";

        static void Main(string[] args)
        {
            authsomeService = new AuthsomeService();
            authsomeService.Provider = new Provider();
            authsomeService.Provider.APIBaseUrl = "https://api.amplitude.com";


            Task.Run(async () =>
            {
                var user_properties = new Dictionary<string, string>();
                user_properties.Add("LoginButton", "Clicked");
                user_properties.Add("CreateUserButton", "Clicked");

                await PostEvent(new List<Event>()
                {
                    new Event() {
                        user_id = "0",
                        event_type = "LoginView",
                        user_properties = user_properties,
                        time = DateTime.Now.Ticks
                    }

                });

                Console.WriteLine("Post Sent!");
                Console.ReadLine();
            });


            Console.ReadLine();
        }









        //public async Task Identify(AmplitudeIdentify identification)
        //{
        //    string data = Newtonsoft.Json.JsonConvert.SerializeObject(identification);

        //    return await trackEvent("identify", "identification", data);
        //}

        public static async Task PostEvent(List<Event> events)
        {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(events);
            await trackEvent("event", data);
        }



        private static async Task trackEvent(string paramName, string paramData)
        {
            var content = new MultipartFormDataContent("----" + DateTime.Now.Ticks);
            var keyContent = new StringContent(apiKey, UTF8Encoding.UTF8, "text/plain");
            content.Add(keyContent, "api_key");

            var data = new StringContent(paramData, UTF8Encoding.UTF8, "application/json");
            content.Add(data, paramName);

            var response = await authsomeService.PostAsync<dynamic>("/httpapi", content);
        }
    }
}
