using ApiServices.Interfaces;
using OpenApi;
using IdentityModel.Client;
using TokenResponse = OpenApi.TokenResponse;
using RefreshTokenRequest = OpenApi.RefreshTokenRequest;

namespace ApiServices.Services
{
    public class IdentityApiService : IIdentityApi, IApiService
    {
        private readonly IdentityApi _identityApi;
        private readonly HttpClient _httpClient;

        public IdentityApiService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _identityApi = new IdentityApi (baseUrl, _httpClient);
        }

        public void InitializeToken(string token)
        {
            _httpClient.SetBearerToken(token);
        }

        public Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            return _identityApi.LoginAsync(request);
        }

        public Task LogoutAsync()
        {
            return _identityApi.LogoutAsync();
        }

        public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return _identityApi.RefreshAsync(request);
        }

        public Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            return _identityApi.RegisterAsync(request);
        }

        public Task RevokeAsync(string userId)
        {
            return _identityApi.RevokeAsync(userId);
        }
    }
}
