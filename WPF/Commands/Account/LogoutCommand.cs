using Desktop.Services.Authentication;
using System;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class LogoutCommand : BaseCommand
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ICommand? _returnCommand;

        public LogoutCommand(AuthenticationService authenticationService, ICommand? returnCommand)
        {
            _authenticationService = authenticationService;
            _returnCommand = returnCommand;
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                await _authenticationService.LogoutAsync();
                _returnCommand?.Execute(null);
            }
            catch (Exception)
            {
                _returnCommand?.Execute(null);
            }          
        }
    }
}
