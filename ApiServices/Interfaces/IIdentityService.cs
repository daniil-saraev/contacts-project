using IdentityApi;

namespace ApiServices.Interfaces
{
    public interface IIdentityService
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);

        Task<TokenResponse> RegisterAsync(RegisterRequest request);

        Task<TokenResponse> RefreshAsync(RefreshTokenRequest request);

        Task RevokeAsync(string userId);

        Task LogoutAsync();
    }
}
