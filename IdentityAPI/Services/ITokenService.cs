using IdentityAPI.Identity;
using IdentityAPI.Responses;

namespace IdentityAPI.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenAsync(ApplicationUser user);

        bool ValidateRefreshToken(string token);
    }
}
