using ApiServices.Identity;
using ApiServices.Interfaces;
using Core.Exceptions.Identity;
using Core.Interfaces;
using Desktop.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Authentication
{
    public class TokenProvider
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenStorage _tokenStorage;

        public TokenProvider(IIdentityApi identityApi, ITokenStorage tokenStorage)
        {
            _identityApi = identityApi;
            _tokenStorage = tokenStorage;
        }

        public async Task<TokenResponse> SendLoginRequest(LoginRequest loginRequest)
        {
            TokenResponse tokenResponse = await _identityApi.LoginAsync(loginRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);

            await _tokenStorage.SaveTokenAsync(tokenResponse);
            return tokenResponse;
        }

        public async Task<TokenResponse> SendRegisterRequest(RegisterRequest registerRequest)
        {
            TokenResponse tokenResponse = await _identityApi.RegisterAsync(registerRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);

            await _tokenStorage.SaveTokenAsync(tokenResponse);
            return tokenResponse;
        }

        public async Task<TokenResponse> SendRefreshRequest(RefreshTokenRequest refreshTokenRequest)
        {
            TokenResponse tokenResponse = await _identityApi.RefreshTokenAsync(refreshTokenRequest);
            if (!tokenResponse.IsSuccessful)
                ThrowException(tokenResponse.ErrorMessage);

            await _tokenStorage.SaveTokenAsync(tokenResponse);
            return tokenResponse;
        }

        public async Task<TokenResponse?> LoadTokenData()
        {
            return await _tokenStorage.GetTokenAsync();
        }

        public async Task RemoveTokenData()
        {
            await _tokenStorage.RemoveTokenAsync();
        }

        private void ThrowException(string errorMessage)
        {
            if (errorMessage == new DuplicateEmailsException().Message)
                throw new DuplicateEmailsException();
            else if (errorMessage == new InvalidRefreshTokenException().Message)
                throw new InvalidRefreshTokenException();
            else if (errorMessage == new RegisterErrorException().Message)
                throw new RegisterErrorException();
            else if (errorMessage == new UserNotFoundException().Message)
                throw new UserNotFoundException();
            else if (errorMessage == new WrongPasswordException().Message)
                throw new WrongPasswordException();
            else
                throw new Exception(errorMessage);
        }
    }
}
