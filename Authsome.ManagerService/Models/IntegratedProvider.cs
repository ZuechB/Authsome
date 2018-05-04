using System.Collections.Generic;

namespace Authsome.ManagerService.Models
{
    public class IntegratedProvider
    {
        public ProviderType ProviderType { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Icon { get; set; }
        public bool IsIntegrated { get; set; }
        public bool AllowMultiple { get; set; }
        public List<OAuthIdentifier> OAuthIdentifiers { get; set; }
    }

    public class OAuthIdentifier
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}