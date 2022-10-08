using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandlers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class RefreshSessionCommand : BaseCommand
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICommand? _returnCommand;

        public RefreshSessionCommand(AuthenticationService authenticationService, IExceptionHandler exceptionHandler, ICommand? returnCommand)
        {
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
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
                _exceptionHandler.HandleException(ex);
            }         
        }
    }
}
