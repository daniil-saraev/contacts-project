using DesktopApp.Exceptions;
using System;
using System.Threading.Tasks;

namespace Desktop.App.Commands.Account.RestoreCommand
{
    public class RestoreSessionCommand : IRestoreSessionCommand
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;

        public RestoreSessionCommand(IAuthenticationService authenticationService, IExceptionHandler exceptionHandler)
        {
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
        }

        public async Task Execute()
        {
            try
            {
                await _authenticationService.RestoreSession();
            }
            catch (ReadingDataException ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }
    }
}
