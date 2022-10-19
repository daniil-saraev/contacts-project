using System;
using System.Windows.Input;

namespace Desktop.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        protected Func<object?, bool>? _canExecuteCustom;

        protected BaseCommand(Func<object?, bool>? canExecuteCustom = null)
        {
            _canExecuteCustom = canExecuteCustom;
        }

        public virtual bool CanExecute(object? parameter)
        {
            return _canExecuteCustom != null ? _canExecuteCustom.Invoke(parameter) : true;
        }

        public abstract void Execute(object? parameter);

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
