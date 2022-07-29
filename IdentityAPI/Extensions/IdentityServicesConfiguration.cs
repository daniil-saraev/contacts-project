using Core.Models.Identity;
using IdentityServer.Data;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Extensions
{
    public static class IdentityServicesConfiguration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.MaxFailedAccessAttempts = 5;

            }).AddEntityFrameworkStores<UserDbContext>()
              .AddDefaultTokenProviders();

            services.AddSingleton<AuthConfiguration>();

            return services;
        }
    }
}
