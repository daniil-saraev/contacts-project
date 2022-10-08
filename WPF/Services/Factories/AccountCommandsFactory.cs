using Desktop.Commands.Account;
using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandlers;
using Desktop.ViewModels.Account;
using System.Windows.Input;

namespace Desktop.Services.Factories
{
    public class AccountCommandsFactory
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IExceptionHandler _exceptionHandler;

        public AccountCommandsFactory(AuthenticationService authenticationService, IExceptionHandler exceptionHandler)
        {
            _authenticationService = authenticationService;
            _exceptionHandler = exceptionHandler;
        }

        public ICommand NewLoginCommand(LoginViewModel loginViewModel, ICommand? returnCommand)
        {
            return new LoginCommand(loginViewModel, _authenticationService, returnCommand);
        }

        public ICommand NewRegisterCommand(RegisterViewModel registerViewModel, ICommand? returnCommand)
        {
            return new RegisterCommand(registerViewModel, _authenticationService, returnCommand);
        }

        public ICommand NewLogoutCommand(ICommand? returnCommand)
        {
            return new LogoutCommand(_authenticationService, returnCommand);
        }

        public ICommand NewRefreshSessionCommand(ICommand? returnCommand)
        {
            return new RefreshSessionCommand(_authenticationService, _exceptionHandler, returnCommand);
        }
    }
}
