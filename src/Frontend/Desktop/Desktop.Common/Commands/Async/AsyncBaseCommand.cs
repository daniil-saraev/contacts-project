using CommunityToolkit.Mvvm.Input;
using Desktop.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Common.Commands.Async
{
    public abstract class AsyncBaseCommand : BaseCommand, IAsyncCommand
    {
        private bool _isCompleted;
        private bool _isRunning;
        protected Task? _executionTask;

        public event PropertyChangedEventHandler? PropertyChanged;
         
        public Task ExecutionTask
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_executionTask);
                return _executionTask;
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            protected set
            {
                _isCompleted = value;
                OnPropertyChanged();
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
            _executionTask = ExecuteAsync();
        }

        public abstract Task ExecuteAsync();

        public override async void Execute(object? parameter)
        {
            try
            {
                await ExecuteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
