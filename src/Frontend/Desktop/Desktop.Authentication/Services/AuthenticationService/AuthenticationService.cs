using Core.Identity.Exceptions;
using Core.Identity.Interfaces;
using Core.Identity.Requests;
using Core.Identity.Responses;
using Desktop.Authentication.Models;
using Desktop.Common.Exceptions;

namespace Desktop.Authentication.Services;

internal class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityService _identityService;
    private readonly IUserDataStorage _userDataStorage;
    private readonly User _user;

    public AuthenticationService(IIdentityService identityService, User user, IUserDataStorage userDataStorage)
    {
        _identityService = identityService;
        _user = user;
        _userDataStorage = userDataStorage;
    }

    public async Task Login(string login, string password)
    {
        LoginRequest request = new LoginRequest
        {
            Email = login,
            Password = password
        };
        var response = await _identityService.LoginAsync(request);
        if (response.IsSuccessful)
            await Authenticate(response);
        else
            ThrowException(response.ExceptionType);
    }

    public async Task Register(string username, string email, string password)
    {
        RegisterRequest request = new RegisterRequest
        {
            Email = email,
            Username = username,
            Password = password
        };
        var response = await _identityService.RegisterAsync(request);
        if (response.IsSuccessful)
            await Authenticate(response);
        else
            ThrowException(response.ExceptionType);
    }

    public async Task Refresh(string refreshToken, string userId)
    {
        RefreshTokenRequest request = new RefreshTokenRequest
        {
            RefreshToken = refreshToken,
            UserId = userId
        };
        var response = await _identityService.RefreshTokenAsync(request);
        if (response.IsSuccessful)
            await Authenticate(response);
        else
            ThrowException(response.ExceptionType);
    }

    public async Task Logout()
    {
        _user.Logout();
        await _userDataStorage.RemoveData();
    }

    private async Task Authenticate(AuthenticationResponse response)
    {
        _user.Authenticate(
            response.AccessToken,
            response.RefreshToken,
            response.UserId,
            response.UserEmail,
            response.UserName);

        await _userDataStorage.SaveData(_user.Data);
    }

    private static void ThrowException(ExceptionType exceptionType)
    {
        switch (exceptionType)
        {
            case ExceptionType.DuplicateEmailsException:
                throw new DuplicateEmailsException();
            case ExceptionType.InvalidRefreshTokenException:
                throw new InvalidRefreshTokenException();
            case ExceptionType.RegisterErrorException:
                throw new RegisterErrorException();
            case ExceptionType.UserLockedOutException:
                throw new UserLockedOutException();
            case ExceptionType.UserNotFoundException:
                throw new UserNotFoundException();
            case ExceptionType.WrongPasswordException:
                throw new WrongPasswordException();
            default:
                throw new Exception("Error");
        }
    }
}