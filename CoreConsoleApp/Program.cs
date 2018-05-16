using Authsome;
using CoreConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            var authsome = new AuthsomeService();

            var providerTest = new Provider();
            providerTest.clientId = "[yourclientid]";
            providerTest.secret = "[yoursecret]";
            providerTest.APIBaseUrl = "https://sandbox-quickbooks.api.intuit.com";
            providerTest.authorizationUrl = "https://appcenter.intuit.com/connect/oauth2?client_id={clientId}&response_type={response_type}&scope={scope}&redirect_uri={redirectUrl}&state={state}";
            providerTest.Id = 1;
            providerTest.RefreshAccessTokenUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
            providerTest.response_type = "code";
            providerTest.RevokeUrl = "";
            providerTest.scope = "com.intuit.quickbooks.accounting,com.intuit.quickbooks.payment,openid,email,profile,address,phone".Split(",");
            providerTest.state = "helloworld";
            providerTest.TokenBearerUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";


            providerTest.TokenResponse = new TokenResponse()
            {
                access_token = "[yourtoken]",
                refresh_token = "[yourrefresh]",
                expires_in = "3600",
                x_refresh_token_expires_in = "8726400",
                token_type = "bearer"
            };

            authsome.Provider = providerTest;
            authsome.InitGlobalRefreshToken((refreshedToken) =>
            {
                if (refreshedToken.httpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    //refreshedToken.Content.access_token -- your new token
                }
            });


            var lineItems = new List<Invoice_Create_Line>();
            lineItems.Add(new Invoice_Create_Line()
            {
                Amount = 248.75f,
                Description = " ",
                DetailType = "SalesItemLineDetail",
                SalesItemLineDetail = new Invoice_Create_Salesitemlinedetail()
                {
                    ItemRef = new Invoice_Create_Itemref() { name = "Decaf Blend - Whole Bean - 12 oz Bags", value = "20" },
                    Qty = 25,
                    UnitPrice = 9.95f,
                }
            });


            var response = await authsome.PostAsync<Invoice_Create_Response_Rootobject>("v3/company/193514608575719/invoice", new Invoice_Create()
            {
                AllowOnlineACHPayment = true,
                CustomerRef = new Invoice_Create_Customerref()
                {
                    value = "3"
                },
                Line = lineItems.ToArray(),
                BillAddr = new Invoice_Create_Billaddr()
                {
                    Line1 = "132 some place cambridge ma 02140"
                },
                ShipAddr = new Invoice_Create_Shipaddr()
                {
                    Line1 = "223 some place cambridge ma 02140"
                },
                BillEmail = new Invoice_Create_Billemail()
                {
                    Address = "someemail@hotmail.com"
                },
                CustomerMemo = new Invoice_Create_Customermemo()
                {
                    value = "919123456789"
                }
            });

            var whatIsTheResponse = response;
        }
    }
}
