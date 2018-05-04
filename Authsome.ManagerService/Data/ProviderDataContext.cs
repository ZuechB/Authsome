using Authsome.ManagerService.Models;
using Microsoft.EntityFrameworkCore;

namespace Authsome.ManagerService.Data
{
    public class ProviderDataContext : DbContext
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<OAuthToken> oAuthTokens { get; set; }
    }
}