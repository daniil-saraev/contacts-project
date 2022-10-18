using Desktop.Commands.Account;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Factories;
using System.Windows.Input;

namespace Desktop.ViewModels.Account
{
    public class AccountViewModel : BaseViewModel
    {
        public string? UserName => User.UserName;
        public string? Email => User.Email;

        public ICommand Return { get; }
        public ICommand Logout { get; }
        public ICommand RefreshSession { get; }

        public AccountViewModel(AuthenticationService authenticationService)
        {
            Return = new ReturnCommand();
            Logout = new LogoutCommand(authenticationService, Return);
            RefreshSession = new RefreshSessionCommand(authenticationService, Return);
        }
    }
}
