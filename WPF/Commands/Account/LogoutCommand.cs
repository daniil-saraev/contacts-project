using Desktop.Services.Authentication;
using System.Windows.Input;

namespace Desktop.Commands.Account
{
    public class LogoutCommand : BaseCommand
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ICommand _returnCommand;

        public LogoutCommand(AuthenticationService authenticationService, ICommand returnCommand)
        {
            _authenticationService = authenticationService;
            _returnCommand = returnCommand;
        }

        public override async void Execute(object? parameter)
        {
            await _authenticationService.LogoutAsync();
            _returnCommand.Execute(null);
        }
    }
}
