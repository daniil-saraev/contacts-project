using Desktop.ViewModels;
using System;
using System.Collections.Generic;
using Desktop.Services.Factories;

namespace Desktop.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IViewModelsFactory _viewModelsFactory;
        private BaseViewModel? _currentViewModel;
        private Stack<BaseViewModel> _viewHistory;

        public BaseViewModel? CurrentViewModel => _currentViewModel;
        public event Action? CurrentViewModelChanged;
        public bool CanReturn => _viewHistory.Count != 0;

        public NavigationService(IViewModelsFactory viewModelsFactory)
        {
            _viewModelsFactory = viewModelsFactory;
            _viewHistory = new Stack<BaseViewModel>();
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            BaseViewModel viewModel = _viewModelsFactory.GetViewModel<T>();
            if (_currentViewModel != null)
                    _viewHistory.Push(_currentViewModel);
                _currentViewModel = viewModel;
                CurrentViewModelChanged?.Invoke();
        }

        public void Return()
        {
            if (CanReturn)
            {
                _currentViewModel = _viewHistory.Pop();
                CurrentViewModelChanged?.Invoke();
            }
        }
    }
}
