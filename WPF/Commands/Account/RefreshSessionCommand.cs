using Desktop.Services.Authentication;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class RefreshSessionCommand : BaseCommand
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ICommand? _returnCommand;

        public RefreshSessionCommand(AuthenticationService authenticationService, ICommand? returnCommand)
        {
            _authenticationService = authenticationService;
            _returnCommand = returnCommand;
        }

        public async override void Execute(object? parameter)
        {
            try
            {
                await _authenticationService.RefreshSessionAsync();
                _returnCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                
            }         
        }
    }
}
