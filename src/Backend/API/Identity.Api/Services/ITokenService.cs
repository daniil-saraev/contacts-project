using Identity.Api.Responses;
using Identity.Models;

namespace Identity.Api.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenResponseAsync(ApplicationUser user);

        bool ValidateRefreshToken(string token);
    }
}
