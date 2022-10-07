using ApiServices.Interfaces;
using Core.Exceptions.Identity;
using Desktop.Services.Authentication.TokenServices;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.Data.FileServices;
using Microsoft.AspNetCore.Mvc.Formatters;
using OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenDecoder _tokenDecoder;
        private readonly IEnumerable<IApiService> _apiServices;
        private readonly UserDataStorage _userDataStorage;

        public IdentityProvider(IIdentityApi identityApi, ITokenDecoder tokenDecoder, IEnumerable<IApiService> apiServices, IFileService<UserData> fileService)
        {
            _identityApi = identityApi;
            _tokenDecoder = tokenDecoder;
            _apiServices = apiServices;
            _userDataStorage = new UserDataStorage(fileService);
        }

        public async Task<IEnumerable<Claim>> LoginAsync(string login, string password)
        {
            LoginRequest loginRequest = new LoginRequest()
            {
                Email = login,
                Password = password
            };
            TokenResponse tokenResponse = await _identityApi.LoginAsync(loginRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);

            return await InitializeNewSessionAsync(tokenResponse);
        }

        public async Task<IEnumerable<Claim>> RegisterAsync(string username, string email, string password)
        {
            RegisterRequest registerRequest = new RegisterRequest()
            {
                Username = username,
                Email = email,
                Password = password
            };
            TokenResponse tokenResponse = await _identityApi.RegisterAsync(registerRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);

            return await InitializeNewSessionAsync(tokenResponse);
        }

        public async Task<IEnumerable<Claim>> RefreshAsync()
        {
            UserData? userData = await _userDataStorage.GetUserDataAsync();
            if (userData == null)
                throw new Exception("User data was not found");

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                RefreshToken = userData.RefreshToken,
                UserId = userData.UserClaims.First(c => c.Type == "id").Value
            };

            TokenResponse tokenResponse = await _identityApi.RefreshAsync(refreshTokenRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);
            return await InitializeNewSessionAsync(tokenResponse);
        }

        public async Task<IEnumerable<Claim>?> RestoreUserAsync()
        {
            UserData? userData = await _userDataStorage.GetUserDataAsync();
            if (userData == null)
                return null;
            else
                return userData.UserClaims;
        }

        public async Task LogoutAsync()
        {
            await _userDataStorage.ClearUserDataAsync();
            await _identityApi.LogoutAsync();
        }

        private async Task<IEnumerable<Claim>> InitializeNewSessionAsync(TokenResponse tokenResponse)
        {
            foreach (IApiService service in _apiServices)
                service.InitializeToken(tokenResponse.AccessToken);

            IEnumerable<Claim> claims = _tokenDecoder.DecodeToken(tokenResponse.AccessToken);
            await _userDataStorage.SaveUserDataAsync(new UserData(tokenResponse.RefreshToken, claims));
            
            return claims;
        }

        private void ThrowException(string errorMessage)
        {
            InvalidRefreshTokenException invalidRefreshTokenException = new InvalidRefreshTokenException();
            UserNotFoundException userNotFoundException = new UserNotFoundException();
            WrongPasswordException wrongPasswordException = new WrongPasswordException();

            if (errorMessage == invalidRefreshTokenException.Message)
                throw invalidRefreshTokenException;
            if (errorMessage == userNotFoundException.Message)
                throw userNotFoundException;
            if (errorMessage == wrongPasswordException.Message)
                throw wrongPasswordException;
        }
    }
}
