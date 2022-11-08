using System;

namespace Desktop.App.Services.Navigation
{
    public interface INavigationService
    {
        public event Action? CurrentViewModelChanged;

        public void NavigateTo<T>() where T : BaseViewModel;

        public void Return();

        public BaseViewModel? CurrentViewModel { get; }

        public bool CanReturn { get; }
    }
}