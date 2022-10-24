using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityAPI.Services;
using System.Security.Claims;
using Core.Exceptions.Identity;
using IdentityAPI.Models;
using IdentityAPI.Models.Requests;
using IdentityAPI.Models.Responses;

namespace IdentityAPI.Controllers
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        private const string REFRESH_TOKEN_CLAIM_TYPE = "refresh_token";

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Tries to sign-in a user based on LoginRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if sign-in was successful. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false</returns>
        [HttpPost("/login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody]LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Error(new UserNotFoundException());
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (!result.Succeeded)
            {
                return Error(new WrongPasswordException());
            }

            return await Authenticate(user);
        }

        /// <summary>
        /// Tries to create a user based on RegisterRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if account was created successfully. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false.</returns>
        [HttpPost("/register")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody]RegisterRequest model)
        {
            if(await _userManager.FindByEmailAsync(model.Email) != null)
                return Error(new DuplicateEmailsException());
            
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
        
            if (!result.Succeeded)
            {
                return Error(new RegisterErrorException());
            }

            return await Authenticate(user);
        }

        /// <summary>
        /// Tries to issue new access and refresh tokens based on RefreshTokenRequest.
        /// </summary>
        /// <returns>TokenResponse with access and refresh tokens if RefreshTokenRequest was validated successfully. 
        /// Otherwise, TokenResponse with error message and IsSuccessful property set to false.</returns>
        [HttpPost("/refresh")]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody]RefreshTokenRequest request)
        {
            var result = _tokenService.ValidateRefreshToken(request.RefreshToken);
            if (result == false)
            {
                return Error(new InvalidRefreshTokenException());
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Error(new UserNotFoundException());
            }

            var refreshTokenClaim = (await _userManager.GetClaimsAsync(user)).First(c => c.Type == REFRESH_TOKEN_CLAIM_TYPE);
            if(refreshTokenClaim == null)
            {
                return Error(new InvalidRefreshTokenException());
            }
            if(refreshTokenClaim.Value != request.RefreshToken)
            {
                return Error(new InvalidRefreshTokenException());
            }

            return await Authenticate(user);
        }

        /// <summary>
        /// Produces a BadRequest response with unsuccessful TokenResponse.
        /// </summary>
        /// <returns>TokenResponse with error message and IsSuccessful property set to false.</returns>
        private ActionResult<TokenResponse> Error(Exception exception)
        {
            TokenResponse response = new TokenResponse();
            response.ErrorMessage = exception.Message;
            response.IsSuccessful = false;
            return Ok(response);
        }

        /// <summary>
        /// Issues claims and creates a successful TokenResponse.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>TokenResponse with access and refresh tokens and IsSuccessful property set to true.</returns>
        private async Task<ActionResult<TokenResponse>> Authenticate(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            if(userClaims.Any(c => c.Type == REFRESH_TOKEN_CLAIM_TYPE))
                await _userManager.RemoveClaimsAsync(user, userClaims.Where(c => c.Type == REFRESH_TOKEN_CLAIM_TYPE));
     
            var tokenResponse = await _tokenService.CreateTokenResponseAsync(user);

            List<Claim> refreshTokenClaim = new List<Claim>
            {
                new Claim(REFRESH_TOKEN_CLAIM_TYPE, tokenResponse.RefreshToken)
            };
            await _userManager.AddClaimsAsync(user, refreshTokenClaim);         

            return Ok(tokenResponse);
        }
    }
}
