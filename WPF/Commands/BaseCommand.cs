using Desktop.Services.ExceptionHandler;
using System;
using System.Windows.Input;

namespace Desktop.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        protected Func<object?, bool>? _canExecuteCustom;
        protected static IExceptionHandler? exceptionHandler;

        protected BaseCommand(Func<object?, bool>? canExecuteCustom = null)
        {
            _canExecuteCustom = canExecuteCustom;
        }

        public virtual bool CanExecute(object? parameter)
        {
            return _canExecuteCustom != null ? _canExecuteCustom.Invoke(parameter) : true;
        }

        public abstract void Execute(object? parameter);

        protected virtual void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public static void SetExceptionHandler(IExceptionHandler exceptionHandler)
        {
            BaseCommand.exceptionHandler = exceptionHandler;
        }
    }
}
