using System.Windows.Input;

namespace Desktop.Common.Commands
{
    /// <summary>
    /// Provides a default implementation of <see cref="ICommand"/> interface.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Allows for dependency injection while not exposing any dependencies.
        /// </summary>
        protected static IServiceProvider ServiceProvider { get; private set; }
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public event EventHandler? CanExecuteChanged;
        protected Func<bool>? _canExecuteCustom;

        /// <summary>
        /// Accepts a custom predicate that defines if the command can be executed.
        /// Note that any logic of refreshing the CanExecute state should be implemented internally through <see cref="OnCanExecuteChanged"/>.
        /// </summary>
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
