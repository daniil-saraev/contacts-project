using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Constants;
using Web.ViewModels;
using static Web.Configuration.TokenNameConstants;
using ApiServices.Interfaces;
using ApiServices.Identity;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityApi _identityApiService;
        
        public AccountController(IIdentityApi identityApiService)
        {
            _identityApiService = identityApiService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl ??= BaseUrls.WEB_CLIENT_URL;

            if (User.Identity != null && User.Identity.IsAuthenticated)
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

            TokenResponse response = await _identityApiService.RegisterAsync(registerRequest);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return View(model);
            }

            AuthenticatePersistent(response);
            return Redirect(model.ReturnUrl);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            HttpContext.Response.Cookies.Delete("refresh_token");
            HttpContext.Session.Remove("access_token");
            return RedirectToAction("Index", "Home");
        }

        private void AuthenticatePersistent(TokenResponse response)
        {
            HttpContext.Response.Cookies.Append(ACCESS_TOKEN, response.AccessToken);
            HttpContext.Response.Cookies.Append(REFRESH_TOKEN, response.RefreshToken);
        }

        private void AuthenticateNonPersistent(TokenResponse response)
        {
            HttpContext.Session.SetString(ACCESS_TOKEN, response.AccessToken);
        }
    }
}
