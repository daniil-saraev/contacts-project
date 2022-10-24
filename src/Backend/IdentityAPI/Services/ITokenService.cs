using IdentityAPI.Models;
using IdentityAPI.Models.Responses;

namespace IdentityAPI.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenResponseAsync(ApplicationUser user);

        bool ValidateRefreshToken(string token);
    }
}
