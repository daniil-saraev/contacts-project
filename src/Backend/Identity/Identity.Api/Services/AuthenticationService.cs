using Core.Identity.Exceptions;
using Core.Identity.Models;
using Core.Identity.Requests;
using Core.Identity.Responses;
using Identity.Api.Configuration;
using Identity.Common.Models;
using Identity.Common.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Core.Identity.Constants.TokenConstants;

namespace Identity.Api.Services
{
    public class AuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtConfiguration _jwtConfiguration;

        public AuthenticationService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
                                    ITokenService tokenService, JwtConfiguration jwtConfiguration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtConfiguration = jwtConfiguration;
        }

        /// <summary>
        /// Tries to sign-in a user.
        /// </summary>
        /// <returns>Successful <see cref="AuthenticationResponse"/>.</returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="UserLockedOutException"></exception>
        /// <exception cref="WrongPasswordException"></exception>
        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (result.IsLockedOut)
            {
                throw new UserLockedOutException();
            }
            if (!result.Succeeded)
            {
                throw new WrongPasswordException();
            }
            
            return await AuthenticateAsync(user);
        }

        /// <summary>
        /// Tries to create a user.
        /// </summary>
        /// <returns>Successful <see cref="AuthenticationResponse"/>.</returns>
        /// <exception cref="DuplicateEmailsException"></exception>
        /// <exception cref="RegisterErrorException"></exception>
        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new DuplicateEmailsException();

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new RegisterErrorException();
            }

            return await AuthenticateAsync(user);
        }

        /// <summary>
        /// Tries to re-authenticate a user with refresh token.
        /// </summary>
        /// <returns>Successful <see cref="AuthenticationResponse"/>.</returns>
        /// <exception cref="InvalidRefreshTokenException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<AuthenticationResponse> RefreshAsync(RefreshTokenRequest request)
        {
            var result = _tokenService.ValidateToken(request.RefreshToken, _jwtConfiguration.RefreshTokenSecret);
            if (result == false)
            {
                throw new InvalidRefreshTokenException();
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var refreshTokenClaim = (await _userManager.GetClaimsAsync(user)).First(c => c.Type == REFRESH_TOKEN);
            if (refreshTokenClaim == null || refreshTokenClaim.Value != request.RefreshToken)
            {
                throw new InvalidRefreshTokenException();
            }

            return await AuthenticateAsync(user);
        }


        private async Task<AuthenticationResponse> AuthenticateAsync(ApplicationUser user)
        {
            var response = CreateAuthenticationResponse(user);
            await SetRefreshTokenClaimAsync(user, response.RefreshToken.Value);
            return response;
        }

        /// <summary>
        /// Generates tokens and composes response.
        /// </summary>
        private AuthenticationResponse CreateAuthenticationResponse(ApplicationUser user)
        {
            IEnumerable<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id) };

            Token accessToken = new Token
            {
                Value = _tokenService.GenerateToken(_jwtConfiguration.AccessTokenSecret, _jwtConfiguration.AccessTokenExpirationMinutes, claims),
                Expiration = DateTime.UtcNow.AddMinutes(_jwtConfiguration.AccessTokenExpirationMinutes)
            };

            Token refreshToken = new Token
            {
                Value = _tokenService.GenerateToken(_jwtConfiguration.RefreshTokenSecret, _jwtConfiguration.RefreshTokenExpirationMinutes, null),
                Expiration = DateTime.UtcNow.AddMinutes(_jwtConfiguration.AccessTokenExpirationMinutes)
            };

            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.UserName,
                IsSuccessful = true
            };
        }

        /// <summary>
        /// Adds or replaces user's refresh token claim.
        /// </summary>
        private async Task SetRefreshTokenClaimAsync(ApplicationUser user, string refreshToken)
        {
            var refreshTokenClaim = new Claim(REFRESH_TOKEN, refreshToken);

            var userClaims = await _userManager.GetClaimsAsync(user);
            if (userClaims.Any(c => c.Type == REFRESH_TOKEN))
                await _userManager.ReplaceClaimAsync(user, userClaims.First(c => c.Type == REFRESH_TOKEN), refreshTokenClaim);
            else
                await _userManager.AddClaimAsync(user, refreshTokenClaim);
        }
    }
}
