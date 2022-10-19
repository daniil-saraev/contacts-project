using IdentityAPI.Data;
using IdentityAPI.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Extensions
{
    public static class IdentityDbInitialization
    {
        public static async Task<WebApplication> UseIdentityInitializationAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                try
                {
                    var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var dbContext = scopedProvider.GetRequiredService<UserDbContext>();
                    await DbInitializer.InitializeAsync(userManager, dbContext);
                    app.Logger.LogInformation("DATABASE INITIALIZED");
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "An error occurred initializing the DB.");
                }
            }
            return app;
        }
    }
}
