using static Core.Identity.Constants.AuthorizationConstants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Identity.Common.Data;
using Identity.Common.Models;

namespace Identity.Common.Configuration
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
            if (await userManager.FindByEmailAsync(EMAIL) == null)
                await userManager.CreateAsync(admin, PASSWORD);
        }
    }
}
