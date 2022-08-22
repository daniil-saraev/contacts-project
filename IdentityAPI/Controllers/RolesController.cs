using Core.Models.Identity;
using IdentityAPI.Data;
using IdentityAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityAPI.Controllers
{
    [Authorize(Policy = Policies.RequireAdmin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public partial class RolesController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(UserManager<ApplicationUser> userManager, UserDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public UserRolesViewModel rolesViewModel { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                claims = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                
                rolesViewModel = new UserRolesViewModel
                {
                    User = user,
                    AllRoles = _dbContext.UserClaims.Select(c => c.ToClaim()).ToList(),
                    UserRoles = (List<Claim>)claims
                };
                return View(rolesViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<Claim> chosenRoles)
        {
            var userRoles = rolesViewModel.UserRoles;
            List<Claim> allRoles = rolesViewModel.AllRoles;
            var addedRoles = chosenRoles.Except(userRoles);
            var removedRoles = allRoles.Except(chosenRoles);

            await _userManager.AddClaimsAsync(rolesViewModel.User, addedRoles);
            await _userManager.RemoveClaimsAsync(rolesViewModel.User, removedRoles);

            return RedirectToAction("Index", "Users");
        }
    }
}
