using Newtonsoft.Json;

namespace Desktop.Common.Services;

/// <summary>
/// An implementation of <see cref="IFileService{T}"/> that uses Json format.
/// </summary>
/// <typeparam name="T"></typeparam>
public class JsonFileService<T> : IFileService<T>
{
    private JsonSerializer serializer;
    private string _filePath;

    /// <summary>
    /// The service binds to the specified path and will only work with that path.
    /// If a file does not exist, it is created.
    /// </summary>
    public JsonFileService(string path)
    {
        serializer = new JsonSerializer();
        _filePath = path;
        if (!File.Exists(path))
            File.Create(path);
    }

    public T? Read()
    {
        T? data;
        using (var streamReader = new StreamReader(_filePath))
        using (JsonReader reader = new JsonTextReader(streamReader))
        {
            data = serializer.Deserialize<T>(reader);
        }
        return data;
    }

    public void Write(T data)
    {
        using (var streamWriter = new StreamWriter(_filePath))
        using (JsonWriter writer = new JsonTextWriter(streamWriter))
        {
            serializer.Serialize(writer, data);
        }
    }

    public void Delete()
    {
        File.Delete(_filePath);
    }
}
