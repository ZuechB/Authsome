using Authsome.ManagerService.Models;
using Microsoft.EntityFrameworkCore;

namespace Authsome.ManagerService.Data
{
    public class ProviderDataContext : DbContext
    {
        public DbSet<Authsome.ManagerService.Models.Provider> Providers { get; set; }
        public DbSet<OAuthToken> OAuthTokens { get; set; }
    }
}