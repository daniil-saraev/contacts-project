using Desktop.Commands;
using Desktop.Services;
using System;

namespace Desktop.Commands.Navigation
{
    public class ReturnCommand : BaseCommand
    {
        private readonly NavigationService _navigation;

        public ReturnCommand(Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigation = NavigationService.GetNavigationService();

        }

        public override void Execute(object? parameter)
        {
            _navigation.Return();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _navigation.CanReturn;
        }
    }
}
