using Desktop.Services;
using Desktop.ViewModels;
using System;

namespace Desktop.Commands.Navigation
{
    public class NavigateCommand : BaseCommand
    {
        private readonly NavigationService _navigation;
        private readonly BaseViewModel _viewModel;

        public NavigateCommand(BaseViewModel viewModel, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigation = NavigationService.GetNavigationService();
            _viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel != null;
        }

        public override void Execute(object? parameter)
        {
            _navigation.SetCurrentViewModel = _viewModel;
        }
    }
}
