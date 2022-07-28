using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityAPI.ViewModels
{
    public class UserRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Claim> AllRoles { get; set; }
        public List<Claim> UserRoles { get; set; }
    }
}
