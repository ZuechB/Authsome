using System.Collections.Generic;

namespace Authsome.ManagerService.Models
{
    public partial class Provider
    {
        public Provider()
        {
            this.OAuthTokens = new HashSet<OAuthToken>();
        }

        public long Id { get; set; }
        public string clientId { get; set; }
        public string secret { get; set; }
        public string redirectUrl { get; set; }
        public string state { get; set; }
        public string authorizationUrl { get; set; }
        public string scope { get; set; }
        public string response_type { get; set; }
        public string RefreshAccessTokenUrl { get; set; }
        public string TokenBearerUrl { get; set; }
        public string APIBaseUrl { get; set; }
        public string RevokeUrl { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Icon { get; set; }
        public bool AllowMultiple { get; set; }
        public bool IsProduction { get; set; }
        public ProviderType ProviderType { get; set; }
        public long PartnerId { get; set; }

        public virtual ICollection<OAuthToken> OAuthTokens { get; set; }
    }
}