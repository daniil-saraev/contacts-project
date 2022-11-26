using System;
using System.Windows.Input;

namespace Desktop.Common.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected static IServiceProvider ServiceProvider { get; private set; }
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public event EventHandler? CanExecuteChanged;
        protected Func<bool>? _canExecuteCustom;

        protected BaseCommand(Func<bool>? canExecuteCustom = null)
        {
            _canExecuteCustom = canExecuteCustom;
        }

        public virtual bool CanExecute(object? parameter)
        {
            return _canExecuteCustom != null ? _canExecuteCustom.Invoke() : true;
        }

        public abstract void Execute(object? parameter);

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
