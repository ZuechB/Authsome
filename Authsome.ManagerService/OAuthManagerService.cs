using Authsome.ManagerService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authsome.ManagerService
{
    public interface IOAuthManagerService
    {
        Task<Provider> GetProvider(ProviderType Type);
        Task<Provider> GetProvider(string Identifier, ProviderType Type);
        Task UpdateToken(ProviderType ProviderId, TokenResponse tokenResponse, string Identifier = null, string IdentifierName = null);
        List<IntegratedProvider> GetAllProviders();
        bool IsProviderActivated(ProviderType Type);
        Task<List<OAuthToken>> GetOAuthConnections(ProviderType Type);
    }

    public class OAuthManagerService : IOAuthManagerService
    {
        readonly Authsome.ManagerService.Data.ProviderDataContext companyContext;

        public bool IsProduction
        {
            get
            {
#if DEBUG
                return false;
#else
                return true;
#endif
            }
        }

        public OAuthManagerService(Authsome.ManagerService.Data.ProviderDataContext companyContext)
        {
            this.companyContext = companyContext;
        }

        public async Task<Provider> GetProvider(ProviderType type)
        {
            var dbProvider = await companyContext.Providers.Include(t => t.OAuthTokens)
                .Where(p => p.ProviderType == type && p.IsProduction == IsProduction).FirstOrDefaultAsync();
            if (dbProvider != null)
            {
                return ConvertProvider(dbProvider);
            }
            else
            {
                return null;
            }
        }

        public async Task<Provider> GetProvider(string Identifier, ProviderType type)
        {
            try
            {
                var provider = await companyContext.Providers.Include(p => p.OAuthTokens)
                    .Where(p => p.ProviderType == type && p.IsProduction == IsProduction).FirstOrDefaultAsync();

                var oAuthToken = provider.OAuthTokens.Where(a => a.Identifier == Identifier).FirstOrDefault();

                var authProvider = ConvertProvider(provider);
                authProvider.TokenResponse = new TokenResponse();

                if (oAuthToken != null)
                {
                    authProvider.TokenResponse = new TokenResponse()
                    {
                        Id = oAuthToken.Id,
                        access_token = oAuthToken.access_token,
                        refresh_token = oAuthToken.refresh_token,
                        expires_in = oAuthToken.expires_in,
                        token_type = oAuthToken.token_type,
                        x_refresh_token_expires_in = oAuthToken.x_refresh_token_expires_in
                    };
                }

                return authProvider;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<OAuthToken>> GetOAuthConnections(ProviderType type)
        {
            var dbProvider = await companyContext.Providers.Include(t => t.OAuthTokens).Where(p => p.ProviderType == type && p.IsProduction == IsProduction).FirstOrDefaultAsync();
            return dbProvider?.OAuthTokens.ToList() ?? new List<OAuthToken>();
        }

        public bool IsProviderActivated(ProviderType Type)
        {
            return companyContext.Providers.Include(t => t.OAuthTokens).Where(p => p.ProviderType == Type && p.IsProduction == IsProduction).Any();
        }

        public List<IntegratedProvider> GetAllProviders()
        {
            var providerList = new List<IntegratedProvider>();
            var providers = companyContext.Providers.Where(p => p.IsProduction == IsProduction).Include(p => p.OAuthTokens);
            foreach (var provider in providers)
            {
                var Identifiers = new List<OAuthIdentifier>();

                if (provider.AllowMultiple)
                {
                    Identifiers.AddRange(provider.OAuthTokens.Select(s => new OAuthIdentifier()
                    {
                        Name = s.IdentifierName,
                        Value = s.Identifier
                    }));
                }

                providerList.Add(new IntegratedProvider()
                {
                    ProviderType = provider.ProviderType,
                    Title = provider.Title,
                    About = provider.About,
                    Icon = provider.Icon,
                    IsIntegrated = provider.OAuthTokens.Any(),
                    AllowMultiple = provider.AllowMultiple,
                    OAuthIdentifiers = Identifiers
                });
            }
            return providerList;
        }

        private Provider ConvertProvider(Authsome.ManagerService.Models.Provider dbProvider)
        {
            var provider = new Provider();
            provider.Id = (long)dbProvider.ProviderType;

            provider.clientId = dbProvider.clientId;
            provider.secret = dbProvider.secret;
            provider.redirectUrl = dbProvider.redirectUrl;
            provider.state = dbProvider.state;
            provider.authorizationUrl = dbProvider.authorizationUrl;
            provider.scope = dbProvider.scope.Split(',');
            provider.response_type = dbProvider.response_type;
            provider.RefreshAccessTokenUrl = dbProvider.RefreshAccessTokenUrl;
            provider.TokenBearerUrl = dbProvider.TokenBearerUrl;
            provider.APIBaseUrl = dbProvider.APIBaseUrl;
            provider.RevokeUrl = dbProvider.RevokeUrl;

            if (dbProvider.OAuthTokens != null)
            {
                var oauthToken = dbProvider.OAuthTokens.FirstOrDefault();
                if (oauthToken != null)
                {
                    provider.TokenResponse = new TokenResponse();
                    provider.TokenResponse.Id = oauthToken.Id;
                    provider.TokenResponse.access_token = oauthToken.access_token;
                    provider.TokenResponse.refresh_token = oauthToken.refresh_token;
                    provider.TokenResponse.x_refresh_token_expires_in = oauthToken.x_refresh_token_expires_in;
                    provider.TokenResponse.expires_in = oauthToken.expires_in;
                    provider.TokenResponse.token_type = oauthToken.token_type;
                }
            }

            return provider;
        }

        public async Task UpdateToken(ProviderType providerType, TokenResponse tokenResponse, string identifier = null, string identifierName = null)
        {
            OAuthToken token;
            var provider = await companyContext.Providers.Include(p => p.OAuthTokens)
                .Where(p => p.ProviderType == providerType && p.IsProduction == IsProduction).FirstOrDefaultAsync();
            if (provider == null)
            {
                throw new Exception("No provider found for that type");
            }

            if (identifier == null)
            {
                if (tokenResponse.Id != null)
                {
                    token = provider.OAuthTokens.Where(p => p.Id == tokenResponse.Id).FirstOrDefault();
                }
                else
                {
                    token = provider.OAuthTokens.FirstOrDefault();
                }
            }
            else
            {
                token = provider.OAuthTokens.Where(p => p.Identifier == identifier).FirstOrDefault();
            }

            if (token != null)
            {
                if (!String.IsNullOrWhiteSpace(tokenResponse.access_token))
                { token.access_token = tokenResponse.access_token; }

                if (!String.IsNullOrWhiteSpace(tokenResponse.refresh_token))
                { token.refresh_token = tokenResponse.refresh_token; }

                if (!String.IsNullOrWhiteSpace(tokenResponse.expires_in))
                { token.expires_in = tokenResponse.expires_in; }

                if (!String.IsNullOrWhiteSpace(tokenResponse.token_type))
                { token.token_type = tokenResponse.token_type; }

                if (!String.IsNullOrWhiteSpace(tokenResponse.x_refresh_token_expires_in))
                { token.x_refresh_token_expires_in = tokenResponse.x_refresh_token_expires_in; }

                token.TokenRenewal = DateTimeOffset.UtcNow;
                //token.PartnerId = PartnerId == -1 ? 1 : PartnerId;

                if (identifier != null)
                {
                    token.Identifier = identifier;
                }

                if (identifierName != null)
                {
                    token.IdentifierName = identifierName;
                }
            }
            else
            {
                companyContext.OAuthTokens.Add(new OAuthToken()
                {
                    access_token = tokenResponse.access_token,
                    expires_in = tokenResponse.expires_in,
                    refresh_token = tokenResponse.refresh_token,
                    token_type = tokenResponse.token_type,
                    x_refresh_token_expires_in = tokenResponse.x_refresh_token_expires_in,
                    ProviderId = provider.Id,
                    //PartnerId = PartnerId == -1 ? 1 : PartnerId,
                    TokenRenewal = DateTimeOffset.UtcNow,
                    Identifier = identifier,
                    IdentifierName = identifierName,

                });
            }

            await companyContext.SaveChangesAsync();
        }
    }
}
