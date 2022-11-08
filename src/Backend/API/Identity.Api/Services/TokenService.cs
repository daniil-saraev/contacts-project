using Identity.Api.Configuration;
using Identity.Api.Responses;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(AuthConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<TokenResponse> CreateTokenResponseAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var accessToken = GenerateToken(_configuration.AccessTokenSecret, _configuration.AccessTokenExpirationMinutes, claims);
            var refreshToken = GenerateToken(_configuration.RefreshTokenSecret, _configuration.RefreshTokenExpirationMinutes, null);
            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsSuccessful = true
            };
        }

        public bool ValidateRefreshToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.RefreshTokenSecret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = _configuration.Issuer,
                    ValidAudience = _configuration.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                }, out var validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GenerateToken(string secret, int expirationMinutes, IEnumerable<Claim>? claims)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expirationMinutes),
                credentials);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return stringToken;
        }
    }
}
