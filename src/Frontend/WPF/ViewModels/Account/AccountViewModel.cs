using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Account;
using Desktop.Services.Authentication;
using Desktop.Services.ExceptionHandler;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.ViewModels.Account
{
    public class AccountViewModel : BaseViewModel
    {
        public string? UserName => User.UserName;
        public string? Email => User.Email;

        public ICommand Return { get; }
        public ICommand Logout { get; }

        public AccountViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IExceptionHandler exceptionHandler)
        {
            Return = new RelayCommand(() => navigationService.Return(), () => navigationService.CanReturn);
            Logout = new LogoutCommand(authenticationService, exceptionHandler, Return);
        }
    }
}
