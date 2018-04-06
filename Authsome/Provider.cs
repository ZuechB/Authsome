namespace Authsome
{
    public class Provider
    {
        public long Id { get; set; }
        public string clientId { get; set; }
        public string secret { get; set; }
        public string redirectUrl { get; set; }
        public string state { get; set; }
        public string authorizationUrl { get; set; } // always like this = client_id={clientId}&response_type={response_type}&scope={scope}&redirect_uri={redirectUrl}&state={state}
        public string[] scope { get; set; }
        public string response_type { get; set; }
        public string RefreshAccessTokenUrl { get; set; }
        public string TokenBearerUrl { get; set; } // often oauth2/token
        public string APIBaseUrl { get; set; } // for api calling (not authentication)
        public string RevokeUrl { get; set; } // Revokes the token


        public TokenResponse TokenResponse { get; set; }
    }
}
