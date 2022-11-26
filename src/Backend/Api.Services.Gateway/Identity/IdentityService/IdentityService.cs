using ApiServices.Identity;
using Core.Identity.Interfaces;
using Core.Identity.Requests;
using Core.Identity.Responses;

namespace Api.Services.Gateway.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityApi _identityApi;
        private readonly HttpClient _httpClient;

        public IdentityService(string baseUrl, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _identityApi = new IdentityApi(baseUrl, _httpClient);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
        {
            var response = await _identityApi.LoginAsync(request);
            return response;
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = await _identityApi.RefreshTokenAsync(request);
            return response;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            var response = await _identityApi.RegisterAsync(request);
            return response;
        }
    }
}
