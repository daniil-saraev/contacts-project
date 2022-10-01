using ApiServices.Interfaces;
using IdentityApi;
using IdentityModel.Client;
using RefreshTokenRequest = IdentityApi.RefreshTokenRequest;
using TokenResponse = IdentityApi.TokenResponse;

namespace ApiServices.Services
{
    public class IdentityService : IIdentityService, IApiService
    {
        private readonly IdentityApiService _identityApiService;
        private readonly HttpClient _httpClient;

        public IdentityService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _identityApiService = new IdentityApiService(baseUrl, _httpClient);
        }

        public void InitializeToken(string token)
        {
            _httpClient.SetBearerToken(token);
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            return await _identityApiService.LoginAsync(request);
        }

        public async Task LogoutAsync()
        {
            await _identityApiService.LogoutAsync();
        }

        public async Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return await _identityApiService.RefreshAsync(request);
        }

        public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            return await _identityApiService.RegisterAsync(request);
        }

        public async Task RevokeAsync(string userId)
        {
            await _identityApiService.RevokeAsync(userId);
        }
    }
}
