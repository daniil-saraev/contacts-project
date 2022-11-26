using Desktop.Common.Exceptions;
using Desktop.Common.Services;
using static Desktop.Authentication.Models.User;

namespace Desktop.Authentication.Services;

internal class UserDataStorage : IUserDataStorage
{
    private readonly IFileService<UserData?> _fileService;

    public UserDataStorage(IFileService<UserData?> fileService)
    {
        _fileService = fileService;
    }

    public async Task<UserData?> LoadData()
    {
        return await Task.Run(() =>
        {
            try
            {
                var data = _fileService.Read();
                return data;
            }
            catch (Exception ex)
            {
                throw new ReadingDataException(ex);
            }
        });
    }

    public async Task SaveData(UserData userData)
    {
        await Task.Run(() =>
        {
            try
            {
                _fileService.Write(userData);
            }
            catch (Exception ex)
            {
                throw new WritingDataException(ex);
            }
        });
    }

    public async Task RemoveData()
    {
        await Task.Run(() =>
        {
            try
            {
                _fileService.Delete();
            }
            catch (Exception ex)
            {
                throw new DataNotFoundException(ex);
            }
        });
    }
}
