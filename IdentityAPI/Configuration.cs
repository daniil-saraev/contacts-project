using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Core.Constants;

namespace IdentityAPI
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(Core.Constants.ApiResources.ContactsDatabaseAPI)      
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource(Core.Constants.ApiResources.ContactsDatabaseAPI)
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = Core.Constants.Clients.ContactsWebClient.WebClientName,
                    ClientSecrets = { new Secret(Core.Constants.Clients.ContactsWebClient.WebClientSecret.ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Core.Constants.ApiResources.ContactsDatabaseAPI                 
                    },
                    RedirectUris = { $"{BaseUrls.WebClientUrl}/Account/Token" },
                    PostLogoutRedirectUris = { $"{BaseUrls.WebClientUrl}/Home/Index" },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true
                }
            };
    }
}
