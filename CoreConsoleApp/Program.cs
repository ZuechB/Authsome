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
                authsome.InitGlobalRefreshToken((refreshedToken) =>
                {
                    if (refreshedToken.httpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //refreshedToken.Content.access_token -- your new token
                    }
                });

                var response = await authsome.GetAsync<BingJson_Rootobject>("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US",
                    (header) =>
                    {
                        header.IncludeAcceptMediaType(MediaType.application_json);
                    });

                if (response.httpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var image = response.Content.images.FirstOrDefault();
                    Console.WriteLine("image url" + image.url);
                }
                else
                {
                    Console.WriteLine(response.httpStatusCode.ToString() + " - " + response.ErrorJson);
                }
                Console.ReadLine();

            }).Wait();
        }
    }
}
