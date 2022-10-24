using ApiServices.Interfaces;
using ApiServices.Identity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication.HttpClientHandlers;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly TokenProvider _tokenProvider;
    private readonly ITokenValidator _tokenValidator;

    public HttpClientAuthorizationDelegatingHandler(TokenProvider tokenProvider, ITokenValidator tokenValidator)
    {
        _tokenProvider = tokenProvider;
        _tokenValidator = tokenValidator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        TokenResponse tokenResponse = await _tokenProvider.LoadTokenData();
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
