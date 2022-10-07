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

        public AccountViewModel(AccountCommandsFactory commandsFactory)
        {
            Return = new ReturnCommand();
            Logout = commandsFactory.NewLogoutCommand(Return);
        }
    }
}
