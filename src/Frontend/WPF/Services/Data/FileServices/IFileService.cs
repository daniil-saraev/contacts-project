namespace Desktop.Services.Data.FileServices
{
    public interface IFileService<T>
    {
        public T? Read();

        public void Write(T data);
    }
}
