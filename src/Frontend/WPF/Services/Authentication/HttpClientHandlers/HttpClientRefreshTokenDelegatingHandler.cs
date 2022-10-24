using ApiServices.Identity;
using ApiServices.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication.HttpClientHandlers;

public class HttpClientRefreshTokenDelegatingHandler
    : DelegatingHandler
{
    private readonly TokenProvider _tokenProvider;
    private readonly ITokenValidator _tokenValidator;
    private readonly TokenStorage _userDataStorage;

    public HttpClientRefreshTokenDelegatingHandler(TokenProvider tokenProvider, ITokenValidator tokenValidator, TokenStorage userDataStorage)
    {
        _tokenProvider = tokenProvider;
        _tokenValidator = tokenValidator;
        _userDataStorage = userDataStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tokenResponse = await _userDataStorage.GetTokenAsync();
        if(tokenResponse != null)
        {
            var isTokenValid = _tokenValidator.ValidateToken(tokenResponse.AccessToken);
            if(!isTokenValid)
            {
                var response = await _tokenProvider.SendRefreshRequest(new RefreshTokenRequest { RefreshToken = tokenResponse.RefreshToken, UserId = User.Id });
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
            }
            else
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}