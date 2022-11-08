using Api.Services.Gateway.Identity;
using Api.Services.Gateway.Identity;

namespace Api.Services.Gateway.Interfaces
{
    public interface IIdentityApi
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);

        Task<TokenResponse> RegisterAsync(RegisterRequest request);

        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
