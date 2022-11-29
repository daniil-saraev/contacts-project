using Desktop.Common.ViewModels;
using System;

namespace Desktop.Common.Services
{

    public interface INavigationService
    {
        public event Action? CurrentViewModelChanged;

        /// <summary>
        /// Sets <see cref="CurrentViewModel"/> to an instance of <typeparamref name="T"/> 
        /// and raises <see cref="CurrentViewModelChanged"/>.
        /// </summary>
        public void NavigateTo<T>() where T : BaseViewModel;

        /// <summary>
        /// A viewmodel to bind to.
        /// </summary>
        public BaseViewModel? CurrentViewModel { get; }

    }
}