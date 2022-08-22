using Desktop.Commands.Navigation;
using Desktop.Services;
using Desktop.ViewModels.Account;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService _navigation;
        public BaseViewModel CurrentViewModel => _navigation.SetCurrentViewModel;

        public ICommand NavigateToAccountView { get; }

        public MainViewModel()
        {
            _navigation = NavigationService.GetNavigationService();
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
            NavigateToAccountView = new NavigateCommand(new AccountViewModel());
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
