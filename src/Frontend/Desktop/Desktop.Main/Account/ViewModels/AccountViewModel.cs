using Desktop.Main.Contacts.ViewModels;
using Desktop.Authentication.Models;
using Desktop.Main.Common.Commands;
using Desktop.Common.ViewModels;
using Desktop.Main.Account.Commands;
using System.Windows.Input;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.Main.Account.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly User _user;

        public string? UserName => _user.Data.Name;
        public string? Email => _user.Data.Email;


        private IAsyncCommand _logout;
        public IRelayCommand Logout { get; }
        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();
        

        public AccountViewModel(User user)
        {
            _user = user;
            _logout = new LogoutCommand(Return);
            LoadingTask = new AsyncRelayCommand(_logout.ExecuteAsync);
            Logout = new RelayCommand(async () =>
            {
                await LoadingTask.ExecuteAsync(null);
            });
        }
    }
}
