using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenApi;
using Core.Constants;
using ApiServices.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityApi _identityApiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IIdentityApi identityApiService, ILogger<AccountController> logger)
        {
            _identityApiService = identityApiService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl ??= BaseUrls.WEB_CLIENT_URL;

            if (User.Identity.IsAuthenticated)
                return Redirect(returnUrl);

            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            LoginRequest loginRequest = new LoginRequest()
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            try
            {
                TokenResponse response = await _identityApiService.LoginAsync(loginRequest);
                if (!response.IsSuccessful)
                {                    
                    ModelState.AddModelError(string.Empty, response.ErrorMessage);
                    return View(model);
                }

                if (model.RememberMe == true)
                    AuthenticatePersistent(response);
                else
                    AuthenticateNonPersistent(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            
            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            returnUrl ??= BaseUrls.WEB_CLIENT_URL;

            if (User.Identity.IsAuthenticated)
                return Redirect(returnUrl);

            RegisterViewModel model = new RegisterViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            RegisterRequest registerRequest = new RegisterRequest()
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            try
            {
                TokenResponse response = await _identityApiService.RegisterAsync(registerRequest);
                if (!response.IsSuccessful)
                {
                    ModelState.AddModelError(string.Empty, response.ErrorMessage);
                    return View(model);
                }

                AuthenticatePersistent(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

            return Redirect(model.ReturnUrl);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            HttpContext.Response.Cookies.Delete("refresh_token");
            HttpContext.Session.Remove("access_token");

            try
            {
                await _identityApiService.LogoutAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            }, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private void AuthenticatePersistent(TokenResponse response)
        {
            HttpContext.Response.Cookies.Append("access_token", response.AccessToken);
            HttpContext.Response.Cookies.Append("refresh_token", response.RefreshToken);
        }

        private void AuthenticateNonPersistent(TokenResponse response)
        {
            HttpContext.Session.SetString("access_token", response.AccessToken);
        }
    }
}
