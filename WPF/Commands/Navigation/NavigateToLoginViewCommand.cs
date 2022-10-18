using Desktop.Services.Authentication;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Account;
using System;

namespace Desktop.Commands.Navigation
{
    public class NavigateToLoginViewCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;
        private readonly IViewModelsFactory _viewModelsFactory;

        public NavigateToLoginViewCommand(IViewModelsFactory viewModelsFactory, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigationService = NavigationService.GetInstance();
            _viewModelsFactory = viewModelsFactory;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _navigationService.CurrentViewModel = _viewModelsFactory.GetViewModel<LoginViewModel>();
        }
    }
}
