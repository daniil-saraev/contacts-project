using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IdentityAPI.ViewModels;
using Core.Constants;

namespace IdentityAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect(BaseUrls.WebClientUrl);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}