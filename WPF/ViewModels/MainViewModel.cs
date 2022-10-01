using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Navigation;
using Desktop.Stores;
using Desktop.ViewModels.Account;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService _navigation;
        private readonly AuthenticationService _authenticationService;
        private readonly ICommand _loadData;
        private readonly ICommand _saveData;

        public BaseViewModel CurrentViewModel => _navigation.CurrentViewModel;    
        public ICommand NavigateToAccountView { get; private set; }

        public MainViewModel(AuthenticationService authenticationService)
        {
            _navigation = NavigationService.GetNavigationService();
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _authenticationService = authenticationService;
            _loadData = new LoadContactsCommand();
            _saveData = new SaveContactsCommand();
            NavigateToAccountView = new NavigateCommand(new LoginViewModel(_authenticationService));
            User.AuthenticationStateChanged += SetAccountView;
        }

        private void SetAccountView()
        {
            if (User.IsAuthenticated)
                NavigateToAccountView = new NavigateCommand(new AccountViewModel(_authenticationService));
            else
                NavigateToAccountView = new NavigateCommand(new LoginViewModel(_authenticationService));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
