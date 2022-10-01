using ApiServices.Interfaces;
using Desktop.Services.Authentication.TokenServices;
using Desktop.Services.Authentication.UserServices;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.ExceptionHandlers;
using IdentityApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly IIdentityService _identityApi;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITokenDecoder _tokenDecoder;
        private readonly IEnumerable<IApiService> _apiServices;
        private readonly User _user;
        private readonly UserDataStorage _userDataStorage;

        public AuthenticationService(IIdentityService identityApi, IExceptionHandler exceptionHandler, ITokenDecoder tokenDecoder, IEnumerable<IApiService> apiServices, IFileService<UserData> fileService)
        {
            _identityApi = identityApi;
            _exceptionHandler = exceptionHandler;
            _tokenDecoder = tokenDecoder;
            _apiServices = apiServices;
            _user = User.GetUser();
            _userDataStorage = new UserDataStorage(fileService);
        }

        public async Task LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _identityApi.LoginAsync(loginRequest);
                InitializeUserSession(response);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(new ExceptionContext(ex));
            }
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var response = await _identityApi.RegisterAsync(registerRequest);
                InitializeUserSession(response);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(new ExceptionContext(ex));
            }
        }     

        public async Task LogoutAsync()
        {
            try
            {
                if (!User.IsAuthenticated)
                    return;              
                _user.Logout();
                _userDataStorage.RemoveUserData();
                await _identityApi.LogoutAsync();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(new ExceptionContext(ex));
            }
        }

        public async Task RevokeAccessAsync()
        {
            try
            {
                if (!User.IsAuthenticated)
                    return;
                await _identityApi.RevokeAsync(User.Id);
                _user.Logout();
                _userDataStorage.RemoveUserData();
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(new ExceptionContext(ex));
            }
        }

        public async Task RefreshSessionAsync()
        {
            try
            {
                _userDataStorage.LoadData();
                var userData = _userDataStorage.GetUserData();
                if (userData == null)
                    return;

                var tokenResponse = userData.CurrentTokenResponse;
                RefreshTokenRequest request = new RefreshTokenRequest
                {
                    RefreshToken = tokenResponse.RefreshToken,
                    UserId = User.Id
                };
                var response = await _identityApi.RefreshAsync(request);
                InitializeUserSession(response);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(new ExceptionContext(ex));
            }
        }

        private void InitializeUserSession(TokenResponse response)
        {
            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessages.First());

            foreach (IApiService service in _apiServices)
                service.InitializeToken(response.AccessToken);

            var claims = _tokenDecoder.DecodeToken(response.AccessToken);
            _user.Authenticate(claims);

            _userDataStorage.SetUserData(new UserData(response));
        }
    }
}
