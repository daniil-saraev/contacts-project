using Core.Models.Identity;
using IdentityAPI.Data;
using IdentityAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityAPI.Controllers
{
    [Authorize(Policy = Policies.RequireAdmin)]
    public partial class RolesController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesViewModel RolesViewModel { get; set; }

        public RolesController(UserManager<ApplicationUser> userManager, UserDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                claims = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                var RolesViewModel = new UserRolesViewModel
                {
                    User = user,
                    AllRoles = _dbContext.UserClaims.Select(c => c.ToClaim()).ToList(),
                    UserRoles = (List<Claim>)claims,
                };
                return View(RolesViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<Claim> roles)
        {
            var userRoles = RolesViewModel.UserRoles;
            List<Claim> allRoles = RolesViewModel.AllRoles;
            var addedRoles = roles.Except(userRoles);
            var removedRoles = allRoles.Except(roles);

            await _userManager.AddClaimsAsync(RolesViewModel.User, addedRoles);
            await _userManager.RemoveClaimsAsync(RolesViewModel.User, removedRoles);

            return RedirectToAction("Index", "Users");
        }
    }
}
