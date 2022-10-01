using Desktop.Commands;
using Desktop.Services.Navigation;
using Desktop.ViewModels;
using System;

namespace Desktop.Commands.Navigation
{
    public class NavigateCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;
        private readonly BaseViewModel _viewModel;

        public NavigateCommand(BaseViewModel viewModel, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigationService = NavigationService.GetNavigationService();
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel != null;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.CurrentViewModel = _viewModel;
        }
    }
}
