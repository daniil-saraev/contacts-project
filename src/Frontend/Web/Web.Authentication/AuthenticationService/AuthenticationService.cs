using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Core.Identity.Constants.TokenConstants;
using Identity.Common.Models;
using Identity.Common.Services;
using Web.Authentication.Configuration;
using Core.Identity.Exceptions;

namespace Web.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtConfiguration jwtConfiguration, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfiguration = jwtConfiguration;
            _tokenService = tokenService;
        }

        public async Task<ClaimsPrincipal> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException();
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (result.Succeeded)
            {
                return await AuthenticateAsync(user);
            }
            if (result.IsLockedOut)
                throw new UserLockedOutException();
            else
                throw new WrongPasswordException();
        }

        public async Task<ClaimsPrincipal> RegisterAsync(string username, string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                throw new DuplicateEmailsException();

            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return await AuthenticateAsync(user);
            }
            else
                throw new RegisterErrorException(result.Errors.First().Description);
        }

        public async Task<ClaimsPrincipal?> RefreshAsync(string accessToken, string email)
        {
            var tokenIsValid = _tokenService.ValidateToken(accessToken, _jwtConfiguration.AccessTokenSecret);
            if (!tokenIsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new UserNotFoundException();

                return await AuthenticateAsync(user);
            }
            else
                return null;
        }

        /// <summary>
        /// Issues access token and sets it as user's claim.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User's <see cref="ClaimsPrincipal"/>.</returns>
        private async Task<ClaimsPrincipal> AuthenticateAsync(ApplicationUser user)
        {
            string accessToken = CreateToken(user);
            await SetAccessTokenClaimAsync(user, accessToken);
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        private string CreateToken(ApplicationUser user)
        {
            IEnumerable<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id) };
            return _tokenService.GenerateToken(_jwtConfiguration.AccessTokenSecret, _jwtConfiguration.AccessTokenExpirationMinutes, claims);
        }

        private async Task SetAccessTokenClaimAsync(ApplicationUser user, string accessToken)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            Claim accessTokenClaim = new Claim(ACCESS_TOKEN, accessToken);
            if (claims.Any(claim => claim.Type == ACCESS_TOKEN))
                await _userManager.ReplaceClaimAsync(user, claims.First(claim => claim.Type == ACCESS_TOKEN), accessTokenClaim);
            else
                await _userManager.AddClaimAsync(user, accessTokenClaim);
        }
    }
}
