using System.Net.Http.Headers;
using Desktop.Authentication.Models;
using Desktop.Authentication.Services;

namespace Desktop.Authentication.HttpClientHandlers;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly User _user;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationHeaderHandler(IAuthenticationService authenticationService, User user)
    {
        _authenticationService = authenticationService;
        _user = user;
    }

    /// <summary>
    /// Validates access token and tries to refresh it if token has expired.
    /// If refreh was unsuccessful, signs user out and throws exception.
    /// Sets the token as authentication header.
    /// </summary>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var data = _user.Data;
        if (data.AccessToken.Expiration <= DateTime.UtcNow)
        {
            try
            {
                await _authenticationService.Refresh(data.RefreshToken.Value, data.Id);
                data = _user.Data;
            }
            catch (Exception)
            {
                await _authenticationService.Logout();
                throw;
            }         
        }
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", data.AccessToken.Value);

        return await base.SendAsync(request, cancellationToken);
    }
}
