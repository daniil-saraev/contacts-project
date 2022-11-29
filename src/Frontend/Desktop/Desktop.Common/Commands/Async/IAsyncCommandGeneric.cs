namespace Desktop.Common.Commands.Async
{
    /// <summary>
    /// Interface for asynchronous command execution.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    public interface IAsyncCommand<T>
    {
        public Task<T> ExecutionTask { get; }
        public bool IsRunning { get; }

        public Task<T> ExecuteAsync();
    }
}
