﻿namespace Authsome
{
    public class TokenResponse
    {
        public long? Id { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string x_refresh_token_expires_in { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
    }

    //public class RefreshedToken
    //{
    //    public HttpResponseWrapper<TokenResponse> RefreshToken { get; set; }
    //    public long ProviderId { get; set; }
    //}
}