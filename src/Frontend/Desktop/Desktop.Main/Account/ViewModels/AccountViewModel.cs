using Desktop.Main.Contacts.ViewModels;
using Desktop.Authentication.Models;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Account.Commands;
using System.Windows.Input;

namespace Desktop.Main.Account.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly User _user;

        public string? UserName => _user.Data.Name;
        public string? Email => _user.Data.Email;

        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();
        public ICommand Logout { get; }

        public AccountViewModel(User user)
        {
            _user = user;
            Logout = new LogoutCommand(Return);
        }
    }
}
