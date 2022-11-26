namespace Desktop.Common.Commands.Async
{
    public interface IAsyncCommand<T>
    {
        public Task<T> ExecutionTask { get; }
        public bool IsCompleted { get; }
        public bool IsRunning { get; }

        public Task<T> ExecuteAsync();
    }
}
