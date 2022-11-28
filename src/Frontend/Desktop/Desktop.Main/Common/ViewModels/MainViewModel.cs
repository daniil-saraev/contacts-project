using System.Windows.Input;
using System.Threading.Tasks;
using Desktop.Common.ViewModels;
using Desktop.Common.Services;
using Desktop.Authentication.Models;
using Desktop.Main.Common.Commands;
using Desktop.Main.Account.ViewModels;
using Desktop.Main.Account.Commands;
using Desktop.Main.Contacts.Commands;
using Desktop.Common.Commands.Async;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Desktop.Main.Contacts.Notifier;
using Desktop.Main.Contacts.ViewModels;

namespace Desktop.Main.Common.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly User _user;
        private readonly INavigationService _navigationService;
        private readonly INotifyUpdateContacts _updateNotifier;
        private readonly IAsyncCommand _loadContacts;
        private readonly IAsyncCommand _saveContacts;
        private readonly IAsyncCommand _restoreUserSession;

        public Visibility ProgressBarVisibility { get; private set; }
        public Visibility ContentVisibility { get; private set; }

        public ICommand NavigateToUserView { get; private set; }
        public BaseViewModel? CurrentViewModel => _navigationService.CurrentViewModel;

        public MainViewModel(User user, INavigationService navigationService, INotifyUpdateContacts notifier)
        {
            _user = user;
            _user.UserLoggedIn += UserLoggedIn;
            _user.UserLoggedOut += UserLoggedOut;
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _updateNotifier = notifier;
            _loadContacts = new LoadContactsCommand();
            _saveContacts = new SaveContactsCommand();
            _restoreUserSession = new RestoreUserSessionCommand();
            NavigateToUserView = new NavigateTo<LoginViewModel>();
        }

        public async void OnStartup()
        {
            LoadingTask = new AsyncRelayCommand(LoadData);
            LoadingTask.PropertyChanged += InitializationTask_PropertyChanged;
            await LoadingTask.ExecuteAsync(null);
            _navigationService.NavigateTo<HomeViewModel>();
            _updateNotifier.Notify();
        }

        private async Task LoadData()
        {
            await _restoreUserSession.ExecuteAsync();
            await _loadContacts.ExecuteAsync();
        }

        private async void UserLoggedIn()
        {
            LoadingTask = new AsyncRelayCommand(_saveContacts.ExecuteAsync);
            LoadingTask.PropertyChanged += InitializationTask_PropertyChanged;
            await LoadingTask.ExecuteAsync(null);
            NavigateToUserView = new NavigateTo<AccountViewModel>();
            OnPropertyChanged(nameof(NavigateToUserView));
            _navigationService.NavigateTo<HomeViewModel>();
            _updateNotifier.Notify();
        }

        private void UserLoggedOut()
        {
            NavigateToUserView = new NavigateTo<LoginViewModel>();
            OnPropertyChanged(nameof(NavigateToUserView));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            if(CurrentViewModel != null)
                LoadingTask = CurrentViewModel.LoadingTask;
            if(LoadingTask != null)
                LoadingTask.PropertyChanged += InitializationTask_PropertyChanged;
        }

        private void InitializationTask_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (LoadingTask.IsRunning)
            {
                ProgressBarVisibility = Visibility.Visible;
                ContentVisibility = Visibility.Collapsed;
            }
            else
            {
                ProgressBarVisibility = Visibility.Collapsed;
                ContentVisibility = Visibility.Visible;
            }          
            OnPropertyChanged(nameof(ProgressBarVisibility));
            OnPropertyChanged(nameof(ContentVisibility));
        }
    }
}
