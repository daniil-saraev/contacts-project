using Desktop.Services.Authentication;

namespace Desktop.Commands.Account
{
    public class RefreshSessionCommand : BaseCommand
    {
        private readonly AuthenticationService _authenticationService;
        
        public RefreshSessionCommand(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public override async void Execute(object? parameter)
        {
            await _authenticationService.RefreshSessionAsync();
        }
    }
}
