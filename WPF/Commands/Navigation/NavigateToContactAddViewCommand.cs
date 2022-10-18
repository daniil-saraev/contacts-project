using Desktop.Containers;
using Desktop.Services.Containers;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Commands.Navigation
{
    public class NavigateToContactAddViewCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;
        private readonly IViewModelsFactory _viewModelsFactory;

        public NavigateToContactAddViewCommand(IViewModelsFactory viewModelsFactory, Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
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
            _navigationService.CurrentViewModel = _viewModelsFactory.GetViewModel<ContactAddViewModel>();
        }
    }
}
