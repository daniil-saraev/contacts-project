using ApiServices.Identity;
using ApiServices.Interfaces;
using Core.Exceptions.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using Web.Configuration;
using static Web.Configuration.TokenNameConstants;

namespace Web.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenValidator _tokenValidator;
        private readonly IIdentityApi _identityApi;

        public TokenMiddleware(RequestDelegate next, ITokenValidator tokenValidator, IIdentityApi identityApi)
        {
            _next = next;
            _tokenValidator = tokenValidator;
            _identityApi = identityApi;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(ACCESS_TOKEN, out string? accessToken);
            accessToken ??= context.Session.GetString(ACCESS_TOKEN);
            context.Request.Cookies.TryGetValue(REFRESH_TOKEN, out string? refreshToken);

            if (!string.IsNullOrEmpty(accessToken))
            {            
                var isValidToken = _tokenValidator.ValidateToken(accessToken);
                if (!isValidToken && refreshToken != null)
                {
                    var tokenResponse = await RefreshToken(context, refreshToken);
                    context.Response.Cookies.Append(ACCESS_TOKEN, tokenResponse.AccessToken);
                    context.Response.Cookies.Append(REFRESH_TOKEN, tokenResponse.RefreshToken);    
                    accessToken = tokenResponse.AccessToken;
                }
                context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            await _next.Invoke(context);
        }

        private async Task<TokenResponse> RefreshToken(HttpContext context, string refreshToken)
        {
            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            RefreshTokenRequest tokenRequest = new RefreshTokenRequest()
            {
                UserId = userId,
                RefreshToken = refreshToken
            };

            TokenResponse response = await _identityApi.RefreshTokenAsync(tokenRequest);
            if (!response.IsSuccessful)
                throw new InvalidRefreshTokenException();
            else
                return response;
        }
    }
}
