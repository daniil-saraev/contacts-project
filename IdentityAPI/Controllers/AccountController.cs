using Core.Constants;
using Core.Models.Identity;
using IdentityAPI.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AccountController(SignInManager<ApplicationUser> signInManager, 
                                 UserManager<ApplicationUser> userManager, 
                                 IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var loginModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(loginModel);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    model.ReturnUrl ??= $"{BaseUrls.WebClientUrl}/Account/Token";
                    return Redirect(model.ReturnUrl);
                }
                ModelState.AddModelError(string.Empty, "Login error");
                return BadRequest();
            }
            ModelState.AddModelError(string.Empty, "ModelState is invalid");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var registerModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(registerModel);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    model.ReturnUrl ??= $"{BaseUrls.WebClientUrl}/Account/Token";
                    Redirect(model.ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest();
            }
            ModelState.AddModelError(string.Empty, "ModelState is invalid");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var result = await _interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(result.PostLogoutRedirectUri))
            {
                return RedirectToAction("Login");
            }
            return Redirect(result.PostLogoutRedirectUri);
        }
    }
}
