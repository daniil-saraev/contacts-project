namespace Desktop.Common.Services;

public interface IFileService<T>
{
    public T? Read();

    public void Write(T data);

    public void Delete();
}
