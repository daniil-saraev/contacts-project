using Core.Exceptions.Identity;
using Desktop.Services.Authentication.Identity;
using Desktop.Services.Authentication.UserServices;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly IIdentityProvider _identityProvider;

        public AuthenticationService(IIdentityProvider identityProvider)
        {
            _identityProvider = identityProvider;
        }

        public async Task LoginAsync(string login, string password)
        {
            var claims = await _identityProvider.LoginAsync(login, password);
            User.Authenticate(claims);
        }

        public async Task RegisterAsync(string username, string email, string password)
        {
            var claims = await _identityProvider.RegisterAsync(username, email, password);
            User.Authenticate(claims);
        }

        public async Task LogoutAsync()
        {
            if (!User.IsAuthenticated)
                return;
            User.Logout();
            await _identityProvider.LogoutAsync();
        }

        public async Task RefreshSessionAsync()
        {
            var claims = await _identityProvider.RestoreUserDataAsync();
            if (claims == null)
                return;

            try
            {
                User.Authenticate(await _identityProvider.RefreshUserDataAsync());
            }
            catch (InvalidRefreshTokenException)
            {
                User.Authenticate(claims);
                throw new InvalidRefreshTokenException();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
