# Authsome

This project allows you to send/receive requests as well as assist with authorization at the same time.

## Nuget

<ol>
    <li>.net Standard: https://www.nuget.org/packages/Authsome</li>
    <li>Portable: https://www.nuget.org/packages/Authsome.Portable</li>
</ol>

# Method Example:

### Return / Response

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
