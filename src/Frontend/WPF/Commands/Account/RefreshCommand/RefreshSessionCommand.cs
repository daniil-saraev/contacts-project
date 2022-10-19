using Desktop.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace Desktop.Commands.Account.Refresh
{
    public class RefreshSessionCommand : IRefreshSessionCommand
    {
        private readonly AuthenticationService _authenticationService;

        public RefreshSessionCommand(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task Refresh()
        {
            try
            {
                await _authenticationService.RefreshSessionAsync();
            }
            catch (Exception ex)
            {
                
            }         
        }
    }
}
