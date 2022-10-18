using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Navigation;
using System.Windows.Input;
using Desktop.Commands.Contacts.LoadCommand;
using Desktop.Commands.Contacts.SaveCommand;
using System.Threading.Tasks;
using Desktop.Commands.Account;
using Desktop.Services.Factories;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly IViewModelsFactory _viewModelsFactory;

        private readonly ILoadCommand _loadContactsCommand;
        private readonly ISaveCommand _saveContactsCommand;
        private readonly ICommand _refreshUserSession;

        public ICommand NavigateToUserView { get; private set; }
        public BaseViewModel CurrentViewModel => _navigation.CurrentViewModel;    
       
        public MainViewModel(AuthenticationService authenticationService, ILoadCommand loadContactsCommand, ISaveCommand saveContactsCommand, INavigationService navigationService)
        {
            CurrentViewModel.NavigateCommandExecuted += Navigate;
            _navigation = navigationService;
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _loadContactsCommand = loadContactsCommand;
            _saveContactsCommand = saveContactsCommand;
            _refreshUserSession = new RefreshSessionCommand(authenticationService, null);
            NavigateToUserView = new NavigateToLoginViewCommand(viewModelsFactory);
            User.AuthenticationStateChanged += SetAccountView;
        }

        public async Task InitializeAsync()
        {
            _refreshUserSession.Execute(null);
            await _loadContactsCommand.Execute();
        }

        public async override void Dispose()
        {
            await _saveContactsCommand.Execute();
        }
        
        private void Navigate(NavigationEventArgs args)
        {
            _navigation.CurrentViewModel = _viewModelsFactory.GetViewModel<typeof()>();
        }

        private void SetAccountView()
        {
            if (User.IsAuthenticated)
                NavigateToUserView = new NavigateToAccountViewCommand(_viewModelsFactory);
            else
                NavigateToUserView = new NavigateToLoginViewCommand(_viewModelsFactory);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
