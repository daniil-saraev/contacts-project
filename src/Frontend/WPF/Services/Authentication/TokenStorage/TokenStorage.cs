using ApiServices.Identity;
using Desktop.Exceptions;
using Desktop.Services.Data.FileServices;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class TokenStorage : ITokenStorage
    {
        private TokenResponse? _tokenResponse;
        private readonly IFileService<TokenResponse> _fileService;

        public TokenStorage(IFileService<TokenResponse> fileService)
        {
            _fileService = fileService;
            _tokenResponse = null;
        }

        public async Task<TokenResponse?> GetTokenAsync()
        {
            if (_tokenResponse == null)
                await LoadTokenFormDisk();

            return _tokenResponse;
        }

        public async Task SaveTokenAsync(TokenResponse userData)
        {
            _tokenResponse = userData;
            await SaveTokenToDisk();
        }

        public async Task RemoveTokenAsync()
        {
            _tokenResponse = null;
            await SaveTokenToDisk();
        }

        private Task LoadTokenFormDisk()
        {
            return Task.Run(() =>
            {
                try
                {
                    var data = _fileService.Read();
                    if (data == null)
                        return;
                    _tokenResponse = data;
                }
                catch (Exception)
                {
                    return;
                }
            });
        }

        private Task SaveTokenToDisk()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (_tokenResponse != null)
                        _fileService.Write(_tokenResponse);
                }
                catch (Exception)
                {
                    return;
                }
            });
        }
    }
}
