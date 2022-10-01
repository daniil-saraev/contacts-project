using Newtonsoft.Json;
using System.IO;

namespace Desktop.Services.DataServices.FileServices
{
    public class JsonFileService<T> : IFileService<T>
    {
        private JsonSerializer serializer;
        private string _filePath;

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
            using (StreamReader streamReader = new StreamReader(_filePath))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                data = serializer.Deserialize<T>(reader);
            }
            return data;
        }

        public void Write(T data)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
