using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Account;
using Desktop.Services.Authentication;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Desktop.ViewModels.Account
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

        public ICommand Return { get; }
        public ICommand Login { get; }
        public ICommand NavigateToRegisterView { get; }

        public LoginViewModel(AuthenticationService authenticationService, INavigationService navigationService)
        {
            Return = new RelayCommand(() => navigationService.NavigateTo<HomeViewModel>());
            Login = new LoginCommand(this, authenticationService, Return);
            NavigateToRegisterView = new RelayCommand(() => navigationService.NavigateTo<RegisterViewModel>());
        }
    }
}
