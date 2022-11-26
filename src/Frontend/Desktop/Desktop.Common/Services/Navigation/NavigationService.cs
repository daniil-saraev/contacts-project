using Desktop.Common.ViewModels;

namespace Desktop.Common.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IViewModelsFactory _viewModelsFactory;
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel => _currentViewModel;
        public event Action? CurrentViewModelChanged;

        public NavigationService(IViewModelsFactory viewModelsFactory, BaseViewModel initialViewModel)
        {
            _viewModelsFactory = viewModelsFactory;
            _currentViewModel = initialViewModel;
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            var viewModel = _viewModelsFactory.GetViewModel<T>();
            _currentViewModel = viewModel;
            CurrentViewModelChanged?.Invoke();
        }
    }
}
