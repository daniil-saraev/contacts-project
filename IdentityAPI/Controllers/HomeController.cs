using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IdentityAPI.ViewModels;

namespace IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}