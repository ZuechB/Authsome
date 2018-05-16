# Authsome

This project allows you to send/receive requests as well as assist with authorization at the same time for OAuth and Basic OAuth.

## Nuget

.net Standard: https://www.nuget.org/packages/Authsome
Portable: https://www.nuget.org/packages/Authsome.Portable

# Method Example:

### Return / Response
<ol>
    <li>response: All responses return HttpResponseWrapper<your-return-object> which includes Content returned back </li>
</ol>

<pre><code>
var response = await authsome.GetAsync<BingJson_Rootobject>("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US");
if (response.httpStatusCode == System.Net.HttpStatusCode.OK)
{
    var image = response.Content.images.FirstOrDefault();
    Console.WriteLine("image url" + image.url);
}
else
{
    Console.WriteLine(response.httpStatusCode.ToString() + " - " + response.ErrorJson);
}
</code></pre>

*** Look at CoreConsoleApp within the solution to see an example of how to generate an invoice within Quickbooks online.
