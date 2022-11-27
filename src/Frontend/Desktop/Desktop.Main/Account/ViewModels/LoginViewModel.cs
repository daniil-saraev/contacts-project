using Desktop.Main.Common.Commands;
using Desktop.Common.Services;
using Desktop.Common.ViewModels;
using Desktop.Main.Account.Commands;
using Desktop.Main.Contacts.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;

namespace Desktop.Main.Account.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                ValidateProperty(value);
                _email = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                ValidateProperty(value);
                _password = value;
                OnPropertyChanged();
            }
        }

        private IAsyncCommand _login;
        public IRelayCommand Login { get; }
        public ICommand NavigateToRegisterView { get; } = new NavigateTo<RegisterViewModel>();
        public ICommand Return { get; } = new NavigateTo<HomeViewModel>();

        public LoginViewModel()
        {
            _login = new LoginCommand(this, Return);
            LoadingTask = new AsyncRelayCommand(_login.ExecuteAsync);
            Login = new RelayCommand(async () =>
            {
                await LoadingTask.ExecuteAsync(null);
            });
        }
    }
}
