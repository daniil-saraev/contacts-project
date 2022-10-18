using Desktop.Commands.Account;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Factories;
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

        public LoginViewModel(AuthenticationService authenticationService, IViewModelsFactory viewModelsFactory)
        {
            Return = new NavigateToHomeViewCommand(viewModelsFactory);
            Login = new LoginCommand(this, authenticationService, Return);
            NavigateToRegisterView = new NavigateToRegisterViewCommand(viewModelsFactory);
        }
    }
}
