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

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            return await _identityApi.LoginAsync(request);
        }

        public async Task LogoutAsync()
        {
            await _identityApi.LogoutAsync();
        }

        public async Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return await _identityApi.RefreshAsync(request);
        }

        public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            return await _identityApi.RegisterAsync(request);
        }

        public async Task RevokeAsync(string userId)
        {
            await _identityApi.RevokeAsync(userId);
        }
    }
}
