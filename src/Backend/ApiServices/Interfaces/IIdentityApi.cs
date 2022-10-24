using ApiServices.Identity;

namespace ApiServices.Interfaces
{
    public interface IIdentityApi
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);

        Task<TokenResponse> RegisterAsync(RegisterRequest request);

        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
