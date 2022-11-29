using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Desktop.Common.Commands.Async
{
    /// <summary>
    /// Default implementation of <see cref="IAsyncCommand"/>.
    /// </summary>
    public abstract class AsyncBaseCommand : BaseCommand, IAsyncCommand
    {
        private bool _isRunning;

        public event PropertyChangedEventHandler? PropertyChanged;     

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public abstract Task ExecuteAsync();

        public override async void Execute(object? parameter)
        {
            try
            {
                IsRunning = true;
                await ExecuteAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsRunning = false;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
