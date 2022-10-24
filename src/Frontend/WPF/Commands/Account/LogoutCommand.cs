using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandler;
using System;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class LogoutCommand : BaseCommand
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public LogoutCommand(IAuthenticationService authenticationService, IExceptionHandler exceptionHandler, ICommand? returnCommand)
        {
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
            _returnCommand = returnCommand;
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                await _authenticationService.Logout();
                _returnCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);     
            }          
        }
    }
}
