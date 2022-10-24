using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;

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
        }
    }
}
