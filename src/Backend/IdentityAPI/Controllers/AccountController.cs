using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityAPI.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityAPI.Identity;
using IdentityAPI.Requests;
using IdentityAPI.Responses;
using Core.Exceptions.Identity;

namespace IdentityAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("[action]")]
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

        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody]RegisterRequest model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return Error(new Exception(result.Errors.First().Description));
            }

            return await Authenticate(user);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponse>> Refresh([FromBody]RefreshTokenRequest request)
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
            var refreshTokenClaim = (await _userManager.GetClaimsAsync(user)).First(c => c.Type == "refresh_token");
            if(refreshTokenClaim == null)
            {
                return Error(new InvalidRefreshTokenException());
            }
            if(refreshTokenClaim.Value != request.RefreshToken)
            {
                await Revoke(user.Id);
            }

            return await Authenticate(user);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Revoke(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();

            var refreshTokenClaim = (await _userManager.GetClaimsAsync(user)).First(c => c.Type == "refresh_token");
            if (refreshTokenClaim == null)
                return BadRequest();

            await _userManager.RemoveClaimAsync(user, refreshTokenClaim);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("[action]")]
        public async Task<ActionResult> Logout()
        {
            string userId = User.FindFirstValue("id");
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();

            var userClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, userClaims.Where(c => c.Type == "refresh_token"));
            return Ok();
        }

        private ActionResult<TokenResponse> Error(Exception exception)
        {
            TokenResponse response = new TokenResponse();
            response.ErrorMessage = exception.Message;
            response.IsSuccessful = false;
            return BadRequest(response);
        }

        private async Task<ActionResult<TokenResponse>> Authenticate(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            if(userClaims.Any(c => c.Type == "refresh_token"))
                await _userManager.RemoveClaimsAsync(user, userClaims.Where(c => c.Type == "refresh_token"));
     
            var response = await _tokenService.CreateTokenAsync(user);

            List<Claim> tokenClaims = new List<Claim>
            {
                new Claim("refresh_token", response.RefreshToken)
            };
            await _userManager.AddClaimsAsync(user, tokenClaims);
            return Ok(response);
        }
    }
}
