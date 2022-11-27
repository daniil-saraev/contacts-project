using System.ComponentModel;
using System.Windows.Input;

namespace Desktop.Common.Commands.Async
{
    public interface IAsyncCommand : INotifyPropertyChanged, ICommand
    {
        public bool IsRunning { get; }

        public Task ExecuteAsync();
    }
}
