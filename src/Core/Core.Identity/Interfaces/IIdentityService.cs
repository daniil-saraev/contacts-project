using Core.Identity.Requests;
using Core.Identity.Responses;
using Core.Common.Exceptions;

namespace Core.Identity.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// Sends a request trying to sign-in a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successful <see cref="AuthenticationResponse"/> if request was successful. Otherwise, returns 
        /// unsuccessful <see cref="AuthenticationResponse"/> with specified <see cref="ExceptionType"/>.
        /// </returns>
        /// <exception cref="ApiException"></exception>
        Task<AuthenticationResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Sends a request trying to create a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successful <see cref="AuthenticationResponse"/> if request was successful. Otherwise, returns 
        /// unsuccessful <see cref="AuthenticationResponse"/> with specified <see cref="ExceptionType"/>.
        /// </returns>
        /// <exception cref="ApiException"></exception>
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Sends a request trying to re-authenticate a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successful <see cref="AuthenticationResponse"/> if request was successful. Otherwise, returns 
        /// unsuccessful <see cref="AuthenticationResponse"/> with specified <see cref="ExceptionType"/>.
        /// </returns>
        /// <exception cref="ApiException"></exception>
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
