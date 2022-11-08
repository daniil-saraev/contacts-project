using static Core.Constants.AuthorizationConstants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Identity.Models;
using Identity.Data.Context;

namespace Identity.Data.Configuration
{
    public static class IdentityDbInitialization
    {
        public static async Task InitializeDbAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IdentityDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var admin = new ApplicationUser { Email = EMAIL, UserName = "Admin" };
            await userManager.CreateAsync(admin, PASSWORD);
        }
    }
}
