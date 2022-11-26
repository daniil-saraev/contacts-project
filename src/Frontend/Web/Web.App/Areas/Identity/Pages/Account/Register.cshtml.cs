using Identity;
using Web.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class RegisterModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Required]
        [StringLength(maximumLength: 64)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 64, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var claimsPrincipal = await _authenticationService.RegisterAsync(Username, Email, Password);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = false
                });
                return Redirect(ReturnUrl);
            }
            catch (DuplicateEmailsException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (RegisterErrorException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}
