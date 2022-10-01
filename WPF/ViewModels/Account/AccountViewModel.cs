using Desktop.Commands.Account;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using System.Windows.Input;

namespace Desktop.ViewModels.Account
{
    public class AccountViewModel : BaseViewModel
    {
        public string? UserName => User.UserName;
        public string? Email => User.Email;

        public ICommand Return { get; }
        public ICommand Logout { get; }

        public AccountViewModel(AuthenticationService authenticationService)
        {
            Return = new ReturnCommand();
            Logout = new LogoutCommand(authenticationService, Return);
        }
    }
}
