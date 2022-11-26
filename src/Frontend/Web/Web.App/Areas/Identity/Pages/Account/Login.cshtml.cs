using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using IAuthenticationService = Web.Authentication.IAuthenticationService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Core.Identity.Exceptions;

namespace Web.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGet(string? returnUrl)
        {
            await HttpContext.SignOutAsync();
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var claimsPrincipal = await _authenticationService.LoginAsync(Email, Password);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = RememberMe
                });
                return Redirect(ReturnUrl);
            }
            catch (UserNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }
            catch (WrongPasswordException)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }
            catch (UserLockedOutException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}
