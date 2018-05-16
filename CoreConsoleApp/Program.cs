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
            providerTest.clientId = "Q0a1yGGytOHNjmmkTJ7NFK0gxHbfoVL3H8efEuARzySQo6smeN";
            providerTest.secret = "mgYnHfvhtkBHatlM3rC6C0wT1ZHDMtseKoC81k8S";
            providerTest.APIBaseUrl = "https://sandbox-quickbooks.api.intuit.com";
            providerTest.authorizationUrl = "https://appcenter.intuit.com/connect/oauth2?client_id={clientId}&response_type={response_type}&scope={scope}&redirect_uri={redirectUrl}&state={state}";
            providerTest.Id = 1;
            providerTest.redirectUrl = "https://leanbox-local.dev.orbose.com/OAuth/OAuthRedirect/1";
            providerTest.RefreshAccessTokenUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
            providerTest.response_type = "code";
            providerTest.RevokeUrl = "";
            providerTest.scope = "com.intuit.quickbooks.accounting,com.intuit.quickbooks.payment,openid,email,profile,address,phone".Split(",");
            providerTest.state = "helloworld";
            providerTest.TokenBearerUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";


            providerTest.TokenResponse = new TokenResponse()
            {
                access_token = "eyJlbmMiOiJBMTI4Q0JDLUhTMadsdsdsdjoiZGlyIn0..WpPMYTB4ivd65xKpDL09Nw.r4BP4qkn33V3qAqJQ3CGZ8x6KhN-D7DPrGagYeFjUUvMzoYjdeieWrDN2bzpa3ITLWstd3vUDNbNdq5JmaG3AKsg7aOJsNZtzWfyEpuXabiJ40cYxwA2D-tJF3BPjhhsStAgSc1kDIZ8h6NtrYcudPlOMGuDX5Smh5jFymLv3HaAJhvI7be7Lus8Jf8pYjnNg3ZdJ69k7GDNoq_PSQ3BQjB6aKBGWpsSWHsjKZNJ8szUIrfTs0OKrOhRTWwqYKK2482kacr-jeUdlXrUhpR7o2WixEQSAD7PEVaNa9yfxMrckWmC1t4xqeqq37VhPcf5UGXFXLwtXWCqR2FN-URdf0Zr-Y_zSQdIyXicy-w8Z3SGD5KtsOGfYYYrn-jndakkwTFjvnzvMtxZ24AQtvhnDUUsf8cLX2Rt-tgpLk5iSQHuyklDT234ECTpym-ayvyqDU5aYSEB_SXydZuzHR-Hdpkr7UdwyftZWWD1k2HXoRaULNlVwtY_kWKQ4h_7-DV4UdMNdlNfpouURLq2XN6RbyXv_hwB_QtJz02oIhKk83n-3reRbBxdn1E2Vjz-hroRpO2ViLOsboMNvjFnpTMU9YnkduuSwuuMcKnczVILU0qAhd0JtZAMyu_2xoDp1K4fWGMk8c1AVbDdQqAw42o6G2q3pem0KwAFs3rymjXtpURSmMs5cgk8L-p0wLYjwC8y90AMLkbR2vDdAuCwy3TTJrH53r4EfjoQA7MRqgAP3qQkYE_bqs7dEjzSSdL2yxJ0v1QLGrCeNdIiIOxU8Lyz1H2ly_xv2gSorkuunL0JZxF8FgczgcyeoH47CUyeNe_yxFn_eYY5Wfj_qjZLaYR9Lw._pWhHBXVy37HH5tbf4hubg",
                refresh_token = "Q011535218867LtlluHEAj5z3Kz41kqrk7mSkKTTyVIuXyo3Bs",
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
                    Line1 = "223 concord turnpike cambridge ma 02140"
                },
                ShipAddr = new Invoice_Create_Shipaddr()
                {
                    Line1 = "223 concord turnpike cambridge ma 02140"
                },
                BillEmail = new Invoice_Create_Billemail()
                {
                    Address = "brandonzuech@hotmail.com"
                },
                CustomerMemo = new Invoice_Create_Customermemo()
                {
                    value = "3362639762"
                }
            });

            var whatIsTheResponse = response;
        }
    }
}
