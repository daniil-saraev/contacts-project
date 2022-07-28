using Desktop.Factories;
using Desktop.ViewModels;
using Desktop.ViewModels.Contacts;
using System;

namespace Desktop.Services
{
    public class NavigationService
    {
        private static NavigationService? _instance;

        private BaseViewModel? _currentViewModel;

        private BaseViewModel? _previousViewModel;

        public event Action? CurrentViewModelChanged;

        public bool CanReturn => _previousViewModel != null;

        public BaseViewModel SetCurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _previousViewModel = _currentViewModel;
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public void Return()
        {
            if(CanReturn)
            {
                _currentViewModel = ViewModelsFactory.GetNewViewModel(_previousViewModel);
                CurrentViewModelChanged?.Invoke();
            }         
        }

        public static NavigationService GetNavigationService()
        {
            if (_instance == null)
                _instance = new NavigationService();
            return _instance;
        }

        private NavigationService()
        {
        }
    }
}
