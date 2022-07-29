using Core.Constants;
using Core.Models.Identity;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IdentityServer.Services
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthConfiguration _configuration;

        public TokenService(UserManager<ApplicationUser> userManager, AuthConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }   
        
        public async Task<TokenResponseModel> CreateToken(ApplicationUser user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);

            var claims = await _userManager.GetClaimsAsync(user);

            JwtSecurityToken token = new JwtSecurityToken(
                BaseUrls.IdentityServerUrl,
                BaseUrls.ContactsDatabaseAPIUrl,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_configuration.TokenExpirationMinutes),
                credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenResponseModel { AccessToken = accessToken };
        }
    }
}
