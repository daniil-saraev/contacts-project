using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.Containers;
using Desktop.ViewModels.Account;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService _navigation;
        private readonly AccountCommandsFactory _commandsFactory;

        public BaseViewModel CurrentViewModel => _navigation.CurrentViewModel;    
        public ICommand NavigateToAccountView { get; private set; }

        public MainViewModel(AccountCommandsFactory commandsFactory)
        {
            _navigation = NavigationService.GetNavigationService();
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _commandsFactory = commandsFactory;                     
            NavigateToAccountView = new NavigateCommand(new LoginViewModel(_commandsFactory));
            User.AuthenticationStateChanged += SetAccountView;
        }

        private void SetAccountView()
        {
            if (User.IsAuthenticated)
                NavigateToAccountView = new NavigateCommand(new AccountViewModel(_commandsFactory));
            else
                NavigateToAccountView = new NavigateCommand(new LoginViewModel(_commandsFactory));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
