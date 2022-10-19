using ApiServices.Interfaces;
using OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Web.Configuration;

namespace Web.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIdentityApi _identityApi;
        private readonly AuthConfiguration _configuration;

        public RefreshTokenMiddleware(RequestDelegate next, IIdentityApi identityApi, AuthConfiguration configuration)
        {
            _next = next;
            _identityApi = identityApi;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string accessToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(accessToken))
            {
                var isValidToken = ValidateToken(accessToken);
                if (!isValidToken)
                {
                    context.Request.Cookies.TryGetValue("refresh_token", out string? refreshToken);
                    string? userId = context.User.FindFirst("id")?.Value;
                    var tokenResponse = await RefreshToken(refreshToken, userId);
                    if (tokenResponse == null)
                    {
                        await _next.Invoke(context);
                        return;
                    }
 
                    context.Response.Cookies.Append("access_token", tokenResponse.AccessToken);
                    context.Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken);
                }
            }
            await _next.Invoke(context);
        }

        private async Task<TokenResponse?> RefreshToken(string? refreshToken, string? userId)
        {
            if (refreshToken == null || userId == null)
                return null;

            RefreshTokenRequest tokenRequest = new RefreshTokenRequest()
            {
                UserId = userId,
                RefreshToken = refreshToken
            };

            TokenResponse response = new TokenResponse();
            try
            {
                response = await _identityApi.RefreshAsync(tokenRequest);
                if (!response.IsSuccessful)
                    return null;
                else
                    return response;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = _configuration.Issuer,
                    ValidAudience = _configuration.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true
                }, out SecurityToken validatedToken); ;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
