using ApiServices.Identity;
using Core.Constants;
using ApiServices.Interfaces;

namespace ApiServices.Services
{
    public class IdentityApiService : IIdentityApi
    {
        private readonly IdentityApi _identityApi;
        private readonly HttpClient _httpClient;

        public IdentityApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _identityApi = new IdentityApi (BaseUrls.IDENTITY_API_URL, _httpClient);
        }

        public Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            return _identityApi.LoginAsync(request);
        }

        public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            return _identityApi.RefreshTokenAsync(request);
        }

        public Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            return _identityApi.RegisterAsync(request);
        }
    }
}
