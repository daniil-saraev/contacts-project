using System.ComponentModel;
using System.Windows.Input;

namespace Desktop.Common.Commands.Async
{
    /// <summary>
    /// Interface for asynchronous command execution.
    /// </summary>
    public interface IAsyncCommand : INotifyPropertyChanged, ICommand
    {
        public bool IsRunning { get; }

        public Task ExecuteAsync();
    }
}
