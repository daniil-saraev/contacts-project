using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Account;
using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandler;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Desktop.ViewModels.Account
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;

        [Required(ErrorMessage = "Username is required")]
        public string Username
        {
            get 
            {
                return _username; 
            }
            set
            {
                ValidateProperty(value);
                _username = value;
                OnPropertyChanged();
            }
        }

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
        [MinLength(6, ErrorMessage = "Minimum password length is 6")]
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

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                ValidateProperty(value);
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand Return { get; }
        public ICommand Register { get; }
        public ICommand NavigateToLoginView { get; }

        public RegisterViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IExceptionHandler exceptionHandler)
        {
            Return = new RelayCommand(() => navigationService.NavigateTo<HomeViewModel>());
            Register = new RegisterCommand(this, authenticationService, exceptionHandler, Return);
            NavigateToLoginView = new RelayCommand(() => navigationService.NavigateTo<LoginViewModel>());
        }
    }
}
