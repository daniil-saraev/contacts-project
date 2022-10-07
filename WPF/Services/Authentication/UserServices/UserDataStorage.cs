using Desktop.Services.Data.FileServices;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication.UserServices
{
    public class UserDataStorage
    {   
        private UserData? _userData;
        private readonly IFileService<UserData> _fileService;

        public UserDataStorage(IFileService<UserData> fileService)
        {
            _fileService = fileService;
        }

        public async Task<UserData?> GetUserDataAsync()
        {
            if(_userData == null)
                await LoadDataFormDisk();

            return _userData;
        }

        public async Task SaveUserDataAsync(UserData userData)
        {
            _userData = userData;
            await SaveDataFromDisk();
        }
        
        public async Task ClearUserDataAsync()
        {
            _userData = null;
            await SaveDataFromDisk();
        }

        private Task LoadDataFormDisk()
        {
            return Task.Run(() =>
            {
                try
                {
                    var data = _fileService.Read();
                    if (data == null)
                        return;
                    _userData = data;
                }
                catch (Exception)
                {
                    return;
                }
            });          
        }

        private Task SaveDataFromDisk()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (_userData != null)
                        _fileService.Write(_userData);
                }
                catch (Exception)
                {
                    return;
                }
            });            
        }     
    }
}
