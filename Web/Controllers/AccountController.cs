using Core.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Core.Interfaces;
using Web.Services;
using ApiServices;

namespace Web.Controllers
{ 
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<Contact> _contactsDb;

        public AccountController(ITokenService tokenService, IRepository<Contact> contactsDb)
        {
            _tokenService = tokenService;
            _contactsDb = contactsDb;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {        
            returnUrl ??= BaseUrls.WebClientUrl;

            if (User.Identity.IsAuthenticated)
                return Redirect(returnUrl);

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            },
            OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            string code = Request.Query["code"];
            code ??= Request.Form["code"];
            var tokenResponse = await _tokenService.GetTokenAsync(code);
            _contactsDb.HttpClient.SetBearerToken(tokenResponse.AccessToken);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                return SignOut(new AuthenticationProperties
                {
                    RedirectUri = BaseUrls.WebClientUrl
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
