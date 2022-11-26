using Desktop.Common.ViewModels;
using System;

namespace Desktop.Common.Services
{
    public interface INavigationService
    {
        public event Action? CurrentViewModelChanged;

        public void NavigateTo<T>() where T : BaseViewModel;

        public BaseViewModel? CurrentViewModel { get; }

    }
}