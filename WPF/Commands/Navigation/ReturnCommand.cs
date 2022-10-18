using Desktop.Commands;
using Desktop.Services.Navigation;
using System;

namespace Desktop.Commands.Navigation
{
    public class ReturnCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;

        public ReturnCommand(Func<object?, bool>? canExecuteCustom = null) : base(canExecuteCustom)
        {
            _navigationService = NavigationService.GetInstance();

        }

        public override void Execute(object? parameter)
        {
            _navigationService.Return();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _navigationService.CanReturn;
        }
    }
}
