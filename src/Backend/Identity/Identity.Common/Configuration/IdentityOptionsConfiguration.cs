using Microsoft.AspNetCore.Identity;

namespace Identity.Common.Configuration
{
    public static class IdentityOptionsConfiguration
    {
        public static void ConfigureDefaultIdentityOptions(IdentityOptions options)
        {
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
        }
    }
}
