using Desktop.Services.DataServices.FileServices;
using Desktop.Services.ExceptionHandlers;
using System;

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

        public UserData? GetUserData()
        {
            return _userData;
        }

        public void SetUserData(UserData userData)
        {
            _userData = userData;
            SaveData();
        }
        
        public void RemoveUserData()
        {
            _userData = null;
            SaveData();
        }

        public void LoadData()
        {
            try
            {
                var data = _fileService.Read();
                if (data == null)
                    return;
                _userData = data;
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveData()
        {
            try
            {
                if(_userData != null)
                    _fileService.Write(_userData);
            }
            catch (Exception ex)
            {

            }
        }     
    }
}
