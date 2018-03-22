using Authsome;
using Authsome.Portable.Models;
using CoreConsoleApp.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var authsome = new AuthsomeService();

                var result = await authsome.GetAsync<BingJson_Rootobject>("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US",
                    (header) =>
                    {
                        header.IncludeUserAgent("Happy Fun User");
                        header.IncludeAcceptMediaType(MediaType.application_json);
                    });

                if (result.httpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var image = result.Content.images.FirstOrDefault();
                    Console.WriteLine("image url" + image.url);
                }
                else
                {
                    Console.WriteLine(result.httpStatusCode.ToString() + " - " + result.ErrorJson);
                }
                Console.ReadLine();

            }).Wait();
        }
    }
}
