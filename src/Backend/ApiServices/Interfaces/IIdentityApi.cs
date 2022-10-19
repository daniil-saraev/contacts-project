using OpenApi;

namespace ApiServices.Interfaces
{
    public interface IIdentityApi
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);

        Task<TokenResponse> RegisterAsync(RegisterRequest request);

        Task<TokenResponse> RefreshAsync(RefreshTokenRequest request);

        Task RevokeAsync(string userId);

        Task LogoutAsync();
    }
}
