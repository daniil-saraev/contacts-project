using System.Net.Http.Headers;
using Core.Identity.Exceptions;
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
