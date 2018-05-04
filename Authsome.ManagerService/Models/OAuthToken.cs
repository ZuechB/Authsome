
namespace Authsome.ManagerService.Models
{
    public partial class OAuthToken
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long PartnerId { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string x_refresh_token_expires_in { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public System.DateTimeOffset TokenRenewal { get; set; }
        public string Identifier { get; set; }
        public string IdentifierName { get; set; }
        public virtual Provider Provider { get; set; }
    }
}