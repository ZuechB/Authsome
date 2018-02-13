# Authsome
Interface that allows you to send requests (including OAuth and Basic Auth) and parse the response in one step and removing all the retry logic, encoding, serialization, refreshing tokens. WARNING: This project is very early and may change completely.


#Method Example:

<ol>
    <li>HttpOption: POST, GET, DELETE, PUT</li>
    <li>URL: Provide the absolute url or relative url (if baseurl is provided in the provider object)</li>
    <li>MYOBJECT: Object that you want to pass as the body</li>
    <li>RefreshedToken: If using OAuth, this will return your token when refreshed. If the provider information is supplied then the method will attempt to refresh the token when the response unauthorized is given back from the server</li>
</ol>
response: All responses return HttpResponseWrapper<your-return-object> to provide you with the content of the object and any status codes and failed responses back from the server

<pre><code>
var response = await authsomeService.Request<Item_Create_Response_Rootobject>(HttpOption.Post, "URL", MYOBJECT, RefreshedToken
    (HttpResponseRefreshToken) =>
    {
        if (HttpResponseRefreshToken.httpStatusCode == HttpStatusCode.OK)
        {
            RenewRefreshToken(HttpResponseRefreshToken.Content);
        }
        else
        {
            WeHaveAnIssue(HttpResponseRefreshToken);
        }
    });
</code></pre>
