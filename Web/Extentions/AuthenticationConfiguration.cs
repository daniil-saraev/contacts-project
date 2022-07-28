using Core.Constants;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace Web.Extentions
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config =>
            {
                config.Authority = BaseUrls.IdentityServerUrl;
                config.ClientId = Clients.ContactsWebClient.WebClientName;
                config.ClientSecret = Clients.ContactsWebClient.WebClientSecret;
                config.SaveTokens = true;
                config.SignedOutCallbackPath = "/Home/Index";
                config.CallbackPath = new PathString("/Account/Token");
                config.ResponseType = OpenIdConnectResponseType.Code;
                config.GetClaimsFromUserInfoEndpoint = true;
                config.ClaimActions.MapJsonKey(ClaimTypes.Role, ClaimTypes.Role);     
                config.Scope.Add(ApiResources.ContactsDatabaseAPI);
                config.Scope.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
                config.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                config.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
            });
            return services;
        }
    }
}
