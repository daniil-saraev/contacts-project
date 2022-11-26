using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Common.Commands.Async
{
    public interface IAsyncCommand : INotifyPropertyChanged
    {
        public Task ExecutionTask { get; }
        public bool IsCompleted { get; }
        public bool IsRunning { get; }

        public Task ExecuteAsync();
    }
}
