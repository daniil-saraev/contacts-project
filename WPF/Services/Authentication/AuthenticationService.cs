using Desktop.Services.Authentication.Identity;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.ExceptionHandlers;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly User _user;

        public AuthenticationService(IIdentityProvider identityProvider, User user)
        {
            _identityProvider = identityProvider;
            _user = user;
        }

        public async Task LoginAsync(string login, string password)
        {
            var claims = await _identityProvider.LoginAsync(login, password);
            _user.Authenticate(claims);
        } 

        public async Task RegisterAsync(string username, string email, string password)
        {
            var claims = await _identityProvider.RegisterAsync(username, email, password);
            _user.Authenticate(claims);
        }     

        public async Task LogoutAsync()
        {
            if (!User.IsAuthenticated)
                return;
            _user.Logout();
            await _identityProvider.LogoutAsync();
        }

        public async Task RestoreSessionAsync()
        {
            var claims = await _identityProvider.RefreshAsync();
            if(claims != null)
            {
                try
                {
                    _user.Authenticate(await _identityProvider.RefreshAsync());
                }
                catch (Exception)
                {                  
                    throw;
                }
                finally
                {
                    _user.Authenticate(claims);
                }
            }             
        }
    }
}
