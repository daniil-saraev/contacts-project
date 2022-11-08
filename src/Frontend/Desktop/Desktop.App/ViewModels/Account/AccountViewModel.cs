using CommunityToolkit.Mvvm.Input;
using Desktop.Commands.Account;
using Desktop.Services.Factories;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.App.ViewModels.Account
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
