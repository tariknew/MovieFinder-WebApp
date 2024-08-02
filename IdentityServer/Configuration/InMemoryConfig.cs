using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
              new List<IdentityResource>
              {
                  new IdentityResources.OpenId(),
                  new IdentityResources.Profile()
              };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("openid")
            };

        public static IEnumerable<Client> GetClients() =>
                new List<Client>
                {
                   new Client
                   {
                        ClientId = "client",
                        ClientSecrets = new [] { new Secret("secret".Sha512()) },
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId },

                        //Token
                        AccessTokenLifetime = 3600,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        SlidingRefreshTokenLifetime = 1296000, /*15 Days*/
                        AbsoluteRefreshTokenLifetime = 0,
                        AllowOfflineAccess = true
                    }
                };
    }
}
