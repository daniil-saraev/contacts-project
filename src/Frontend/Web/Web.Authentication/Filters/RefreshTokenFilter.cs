using System.Security.Claims;
using static Core.Identity.Constants.TokenConstants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Authentication;

[Authorize]
public class RefreshTokenFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenFilter(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Tries to refresh token. If refresh fails, redirects user to login page.
    /// </summary>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string? accessToken = context.HttpContext.User.FindFirstValue(ACCESS_TOKEN);
        string? email = context.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(email))
            await context.HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        try
        {
            var claimsPrincipal = await _authenticationService.RefreshAsync(accessToken, email);
            if (claimsPrincipal != null)
            {
                ClaimsIdentity? identity = (ClaimsIdentity?)context.HttpContext.User.Identity;
                identity?.RemoveClaim(identity.FindFirst(claim => claim.Type == ACCESS_TOKEN));
                identity?.AddClaim(claimsPrincipal.FindFirst(ACCESS_TOKEN));
            }
        }
        catch (Exception)
        {
            await context.HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

