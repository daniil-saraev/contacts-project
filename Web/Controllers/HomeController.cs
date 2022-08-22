using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Core.Constants;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return RedirectToAction("Index", "Contact");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Users()
        {
            return Redirect($"{BaseUrls.IdentityServerUrl}/Users/Index");
        }
    }
}
