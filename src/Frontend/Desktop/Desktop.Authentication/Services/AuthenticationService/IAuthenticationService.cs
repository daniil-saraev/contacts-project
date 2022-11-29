using Core.Identity.Exceptions;

namespace Desktop.Authentication.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// Tries to authenticate a user. If successful, saves user data locally.
    /// </summary>
    /// <exception cref="UserLockedOutException"></exception>
    /// <exception cref="UserNotFoundException"></exception>
    /// <exception cref="WrongPasswordException"></exception>
    Task Login(string login, string password);

    /// <summary>
    /// Tries to create a user. If successful, saves user data locally.
    /// </summary>
    /// <exception cref ="DuplicateEmailsException"></exception>
    /// <exception cref="RegisterErrorException"></exception>
    Task Register(string username, string email, string password);

    /// <summary>
    /// Signs user out. Clears user's local data.
    /// </summary>
    /// <returns></returns>
    Task Logout();

    /// <summary>
    /// Tries to re-authenticate a user with refresh token. If successful, updates and saves user data locally.
    /// </summary>
    /// <exception cref="UserNotFoundException"></exception>
    /// <exception cref="InvalidRefreshTokenException"></exception>
    Task Refresh(string refreshToken, string userId);
}