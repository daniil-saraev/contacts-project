using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Desktop.Common.Commands.Async
{
    /// <summary>
    /// Default implementation of <see cref="IAsyncCommand{T}"/>.
    /// </summary>
    /// <typeparam name="T">Return type.</typeparam>
    public abstract class AsyncBaseCommand<T> : BaseCommand, IAsyncCommand<T>
    {
        private bool _isRunning;
        protected Func<Task<T>>? _executionTask;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Task<T> ExecutionTask
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_executionTask);
                return _executionTask.Invoke();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        protected AsyncBaseCommand()
        {
            _executionTask = ExecuteAsync;
        }

        public abstract Task<T> ExecuteAsync();

        public override async void Execute(object? parameter)
        {
            try
            {
                await ExecuteAsync();
            }
            catch (Exception)
            {
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
