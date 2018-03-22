# Authsome
Interface that allows you to send requests (including OAuth and Basic Auth) and parse the response in one step and removing all the retry logic, encoding, serialization, refreshing tokens. WARNING: This project is very early and may change completely.

## Nuget

https://www.nuget.org/packages/Authsome.Portable

# Method Example:

### Return / Response
<ol>
    <li>response: All responses return HttpResponseWrapper<your-return-object> to provide you with the content of the object and any status codes and failed responses back from the server</li>
</ol>

<pre><code>
var response = await authsome.GetAsync<BingJson_Rootobject>("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US",
        (header) =>
        {
            header.IncludeUserAgent("Happy Fun User");
            header.IncludeAcceptMediaType(MediaType.application_json);
        });

    if (response.httpStatusCode == System.Net.HttpStatusCode.OK)
    {
        var image = response.Content.images.FirstOrDefault();
        Console.WriteLine("image url" + image.url);
    }
    else
    {
        Console.WriteLine(response.httpStatusCode.ToString() + " - " + result.ErrorJson);
    }
</code></pre>
