using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Api.Services;
using Core.Identity.Responses;
using Core.Identity.Requests;
using Core.Identity.Exceptions;

namespace Identity.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AccountController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Tries to sign-in a user based on LoginRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if sign-in was successful. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false</returns>
        [HttpPost("/login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authenticationService.LoginAsync(request);
                return Ok(response);
            }
            catch (UserNotFoundException)
            {
                return Error(ExceptionType.UserNotFoundException);
            }
            catch (WrongPasswordException)
            {
                return Error(ExceptionType.WrongPasswordException);
            }
            catch (UserLockedOutException)
            {
                return Error(ExceptionType.UserLockedOutException);
            }
        }

        /// <summary>
        /// Tries to create a user based on RegisterRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if account was created successfully. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false.</returns>
        [HttpPost("/register")]
        public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authenticationService.RegisterAsync(request);
                return Ok(response);
            }
            catch (DuplicateEmailsException)
            {
                return Error(ExceptionType.DuplicateEmailsException);
            }
            catch (RegisterErrorException)
            {
                return Error(ExceptionType.RegisterErrorException);
            }
        }

        /// <summary>
        /// Tries to issue new access and refresh tokens based on RefreshTokenRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if RefreshTokenRequest was validated successfully. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false.</returns>
        [HttpPost("/refresh")]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authenticationService.RefreshAsync(request);
                return Ok(response);
            }
            catch (UserNotFoundException)
            {
                return Error(ExceptionType.UserNotFoundException);
            }
            catch (InvalidRefreshTokenException)
            {
                return Error(ExceptionType.InvalidRefreshTokenException);
            }
        }

        private ActionResult<AuthenticationResponse> Error(ExceptionType exceptionType)
        {
            var response = new AuthenticationResponse();
            response.IsSuccessful = false;
            response.ExceptionType = exceptionType;
            return Ok(response);
        }
    }
}
