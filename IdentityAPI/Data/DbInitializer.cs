using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityAPI.Data
{
    public static class DbInitializer
    {
        private const string adminEmail = "admin@mail.ru";
        private const string adminName = "admin";
        private const string adminPasswod = "Admin123";

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, UserDbContext context)
        {          
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            ApplicationUser admin = new ApplicationUser { UserName = adminName, Email = adminEmail };
            IdentityResult result = await userManager.CreateAsync(admin, adminPasswod);
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(admin, ClaimStore.AdminClaim);
            }

            ApplicationUser Ivan = new ApplicationUser { UserName = "Ivan", Email = "ivan@mail.ru" };
            await userManager.CreateAsync(Ivan, "Ivan123");

            ApplicationUser Petr = new ApplicationUser { UserName = "Petr", Email = "petr@mail.ru" };
            await userManager.CreateAsync(Petr, "Petr123");
        }
    }
}
