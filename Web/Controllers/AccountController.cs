using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityApi;
using Core.Constants;
using ApiServices.Interfaces;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityApiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IIdentityService identityApiService, ILogger<AccountController> logger)
        {
            _identityApiService = identityApiService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl ??= BaseUrls.WebClientUrl;

            if (User.Identity.IsAuthenticated)
                return Redirect(returnUrl);

            LoginRequest model = new LoginRequest
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if(!ModelState.IsValid)
                return View(model);

            TokenResponse response = new TokenResponse();
            try
            {
                response = await _identityApiService.LoginAsync(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            if(!response.IsSuccessful)
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            if (model.RememberMe == true)
                AuthenticatePersistent(response);
            else
                AuthenticateNonPersistent(response);

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            returnUrl ??= BaseUrls.WebClientUrl;

            if (User.Identity.IsAuthenticated)
                return Redirect(returnUrl);

            RegisterRequest model = new RegisterRequest
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TokenResponse response = new TokenResponse();
            try
            {
                response = await _identityApiService.RegisterAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            if (!response.IsSuccessful)
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            AuthenticatePersistent(response);

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
                _logger.LogError(ex.Message);
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
