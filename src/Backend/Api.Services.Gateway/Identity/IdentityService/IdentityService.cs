using Core.Common.Constants;
using Core.Common.Exceptions;
using Core.Identity.Interfaces;
using Core.Identity.Requests;
using Core.Identity.Responses;

namespace Api.Services.Gateway.Identity
{
    /// <summary>
    /// Wrapper around Identity.Api client.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IdentityApi _identityApi;
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _identityApi = new IdentityApi(BaseUrls.IDENTITY_API_URL, _httpClient);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _identityApi.LoginAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }           
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var response = await _identityApi.RefreshTokenAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _identityApi.RegisterAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }         
        }
    }
}
