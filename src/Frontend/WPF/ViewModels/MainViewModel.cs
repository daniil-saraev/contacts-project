using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Navigation;
using System.Windows.Input;
using Desktop.Commands.Contacts.LoadCommand;
using System.Threading.Tasks;
using Desktop.Commands.Account;
using Desktop.Services.Factories;
using CommunityToolkit.Mvvm.Input;
using Desktop.ViewModels.Account;
using Desktop.Commands.Account.Refresh;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ILoadContactsCommand _loadContactsCommand;
        private readonly IRefreshSessionCommand _refreshUserSession;

        public ICommand NavigateToUserView { get; private set; }
        public BaseViewModel? CurrentViewModel => _navigationService.CurrentViewModel;    
       
        public MainViewModel(IRefreshSessionCommand refreshSessionCommand, ILoadContactsCommand loadContactsCommand, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _loadContactsCommand = loadContactsCommand;
            _refreshUserSession = refreshSessionCommand;
            NavigateToUserView = new RelayCommand(() => navigationService.NavigateTo<LoginViewModel>());
            User.AuthenticationStateChanged += SetAccountView;
        }

        public async Task InitializeAsync()
        {
            await _refreshUserSession.Refresh();
            await _loadContactsCommand.Load();
        }

        private void SetAccountView()
        {
            if (User.IsAuthenticated)
                NavigateToUserView = new RelayCommand(() => _navigationService.NavigateTo<AccountViewModel>());
            else
                NavigateToUserView = new RelayCommand(() => _navigationService.NavigateTo<LoginViewModel>());
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
