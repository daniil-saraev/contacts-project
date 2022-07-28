using IdentityModel.Client;
using Core.Constants;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Web.Services
{
    public class AuthorizationCodeTokenService : ITokenService
    {
        private readonly DiscoveryDocumentResponse _discoveryDocument;
        private readonly HttpClient _httpClient;
        private TokenResponse? _currentTokenResponse;

        public AuthorizationCodeTokenService()
        {
            _httpClient = new HttpClient();
            _discoveryDocument = _httpClient.GetDiscoveryDocumentAsync(BaseUrls.IdentityServerUrl).Result;
            if (_discoveryDocument.IsError)
            {
                throw new Exception("Error getting DiscoveryDocument", _discoveryDocument.Exception);
            }
        }

        public async Task<TokenResponse> GetTokenAsync(params string[] args) 
        {
            var tokenResponse = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientId = Clients.ContactsWebClient.WebClientName,
                ClientSecret = Clients.ContactsWebClient.WebClientSecret,
                Code = args[0],
                RedirectUri = BaseUrls.WebClientUrl
            });

            if(tokenResponse.IsError)
            {
                throw new Exception("Error getting token", tokenResponse.Exception);
            }    
            _currentTokenResponse = tokenResponse;
            return tokenResponse;
        }

        public async Task<TokenResponse> RefreshToken()
        {
            var tokenResponse = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientId = Clients.ContactsWebClient.WebClientName,
                ClientSecret = Clients.ContactsWebClient.WebClientSecret,
                RefreshToken = _currentTokenResponse.RefreshToken
            });
            return tokenResponse;
        }
    }
}
