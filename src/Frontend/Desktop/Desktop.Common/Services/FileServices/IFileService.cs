namespace Desktop.Common.Services;

/// <summary>
/// A service for working with data on hard drive.
/// </summary>
/// <typeparam name="T">Type to serialize/deserialize.</typeparam>
public interface IFileService<T>
{
    /// <summary>
    /// Reads and deserializes data from disk.
    /// </summary>
    /// <returns><see cref="T"/> if any data was found, otherwise null.</returns>
    public T? Read();

    /// <summary>
    /// Serializes and writes data to disk.
    /// </summary>
    public void Write(T data);

    /// <summary>
    /// Removes a file.
    /// </summary>
    public void Delete();
}
