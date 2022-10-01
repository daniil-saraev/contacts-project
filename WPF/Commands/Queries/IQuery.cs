using System.Threading.Tasks;

namespace Desktop.Commands.Queries
{
    /// <summary>
    /// Represents a command that returns a value
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    public interface IQuery<T>
    {
        Task<T?> Execute(object? parameter = null);
    }
}
