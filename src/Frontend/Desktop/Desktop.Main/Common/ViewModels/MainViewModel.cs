using System.Windows.Input;
using System.Threading.Tasks;
using Desktop.Common.ViewModels;
using Desktop.Common.Services;
using Desktop.Authentication.Models;
using Desktop.Main.Common.Commands;
using Desktop.Main.Account.ViewModels;
using Desktop.Main.Account.Commands;
using Desktop.Main.Contacts.Commands;
using Nito.AsyncEx;
using Desktop.Common.Commands.Async;
using Desktop.Main.Contacts.ViewModels;

namespace Desktop.Main.Common.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly User _user;
        private readonly INavigationService _navigationService;
        private readonly IAsyncCommand _loadContacts;
        private readonly IAsyncCommand _restoreUserSession;

        private INotifyTaskCompletion _initializationTask;
        public INotifyTaskCompletion InitializationTask
        {
            get => _initializationTask;
            private set
            {
                _initializationTask = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToUserView
        {
            get
            {
                if (_user.IsAuthenticated)
                    return new NavigateTo<AccountViewModel>();
                else
                    return new NavigateTo<LoginViewModel>();
            }
        }
        public BaseViewModel? CurrentViewModel => _navigationService.CurrentViewModel;

        public MainViewModel(User user, INavigationService navigationService)
        {
            _user = user;
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _loadContacts = new LoadContactsCommand();
            _restoreUserSession = new RestoreUserSessionCommand();
            _initializationTask = NotifyTaskCompletion.Create(LoadData());
        }

        public async void OnStartup()
        {
            await LoadData();        
        }

        private async Task LoadData()
        {           
            await Task.Delay(3000);
            await _restoreUserSession.ExecuteAsync();
            await _loadContacts.ExecuteAsync();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
