using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using Desktop.Services.Authentication;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.Containers;
using Desktop.ViewModels.Account;
using System.Windows.Input;
using Desktop.Commands.Contacts.LoadCommand;
using Desktop.Commands.Contacts.SaveCommand;
using System.Threading.Tasks;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService _navigation;
        private readonly AccountCommandsFactory _commandsFactory;

        private readonly ILoadCommand _loadContactsCommand;
        private readonly ISaveCommand _saveContactsCommand;
        private readonly ICommand _refreshUserSession;

        public ICommand NavigateToAccountView { get; private set; }
        public BaseViewModel CurrentViewModel => _navigation.CurrentViewModel;    
       
        public MainViewModel(AccountCommandsFactory commandsFactory, ILoadCommand loadContactsCommand, ISaveCommand saveContactsCommand)
        {
            _navigation = NavigationService.GetNavigationService();
            _commandsFactory = commandsFactory;
            _loadContactsCommand = loadContactsCommand;
            _saveContactsCommand = saveContactsCommand;
            _refreshUserSession = commandsFactory.NewRefreshSessionCommand(null);
            NavigateToAccountView = new NavigateCommand(new LoginViewModel(commandsFactory));
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
            User.AuthenticationStateChanged += SetAccountView;
        }

        public async Task InitializeAsync()
        {
            _refreshUserSession.Execute(null);
            await _loadContactsCommand.Execute();
        }

        public override void Dispose()
        {
            base.Dispose();
            _saveContactsCommand.Execute();
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
