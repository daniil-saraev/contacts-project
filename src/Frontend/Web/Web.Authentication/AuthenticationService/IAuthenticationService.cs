using Core.Identity.Exceptions;
using System.Security.Claims;

namespace Web.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Tries to sign-in a user.
        /// </summary>
        /// <returns><see cref="ClaimsPrincipal"/> of the user.</returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="UserLockedOutException"></exception>
        /// <exception cref="WrongPasswordException"></exception>
        Task<ClaimsPrincipal> LoginAsync(string email, string password);

        /// <summary>
        /// Tries to create a user.
        /// </summary>
        /// <returns><see cref="ClaimsPrincipal"/> of the created user.</returns>
        /// <exception cref="DuplicateEmailsException"></exception>
        /// <exception cref="RegisterErrorException"></exception>
        Task<ClaimsPrincipal> RegisterAsync(string username, string email, string password);

        /// <summary>
        /// Checks if access token is valid and issues new token if validation failed.
        /// </summary>
        /// <returns>Updated <see cref="ClaimsPrincipal"/> of the user if tokens were refreshed, otherwise null.</returns>
        /// <exception cref="UserNotFoundException"></exception>
        Task<ClaimsPrincipal?> RefreshAsync(string accessToken, string email);
    }
}
