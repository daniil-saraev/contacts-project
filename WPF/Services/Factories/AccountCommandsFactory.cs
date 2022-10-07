using Desktop.Commands.Account;
using Desktop.Services.Authentication;
using Desktop.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Services.Factories
{
    public class AccountCommandsFactory
    {
        private readonly AuthenticationService _authenticationService;

        public AccountCommandsFactory(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ICommand NewLoginCommand(LoginViewModel loginViewModel)
        {
            return new LoginCommand(loginViewModel, _authenticationService);
        }

        public ICommand NewRegisterCommand(RegisterViewModel registerViewModel)
        {
            return new RegisterCommand(registerViewModel, _authenticationService);
        }

        public ICommand NewLogoutCommand(ICommand? returnCommand)
        {
            return new LogoutCommand(_authenticationService, returnCommand);
        }
    }
}
