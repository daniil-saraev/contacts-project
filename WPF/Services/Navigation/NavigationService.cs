using Desktop.Factories;
using Desktop.ViewModels;
using System;
using System.Collections.Generic;

namespace Desktop.Services.Navigation
{
    public class NavigationService
    {
        private static NavigationService? _instance;
        private readonly ViewModelsFactory _viewModelsFactory;

        private BaseViewModel? _currentViewModel;

        private Stack<BaseViewModel> _viewHistory;

        public event Action? CurrentViewModelChanged;

        public bool CanReturn => _viewHistory.Count != 0;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel != null)
                    _viewHistory.Push(_currentViewModel);
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public void Return()
        {
            if (CanReturn)
            {
                _currentViewModel = _viewModelsFactory.GetNewViewModel(_viewHistory.Pop());
                CurrentViewModelChanged?.Invoke();
            }
        }

        public static void InitializeNavigationService(ViewModelsFactory viewModelsFactory)
        {
            _instance ??= new NavigationService(viewModelsFactory);
        }

        public static NavigationService GetNavigationService()
        {
            if (_instance == null)
                throw new Exception("Navigation service was not initialized!");
            else
                return _instance;
        }

        private NavigationService(ViewModelsFactory viewModelsFactory)
        {
            _viewModelsFactory = viewModelsFactory;
            _viewHistory = new Stack<BaseViewModel>();
        }
    }
}
