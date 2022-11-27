using Desktop.Authentication.Services;
using Desktop.Common.Commands.Async;
using Desktop.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Main.Account.Commands
{
    public class LogoutCommand : AsyncBaseCommand
    {
        private IAuthenticationService _authenticationService => ServiceProvider.GetRequiredService<IAuthenticationService>();
        private IExceptionHandler _exceptionHandler => ServiceProvider.GetRequiredService<IExceptionHandler>();

        private readonly ICommand? _returnCommand;

        public LogoutCommand(ICommand? returnCommand)
        {
            _returnCommand = returnCommand;
        }

        public override async Task ExecuteAsync()
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
