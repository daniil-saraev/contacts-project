using Identity.Api.Services;
using Identity.Common.Data;
using Identity.Common.Models;
using Identity.Common.Services;
using Identity.Common.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Configuration
{
    internal static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserDbConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                IdentityOptionsConfiguration.ConfigureDefaultIdentityOptions(options);

            }).AddEntityFrameworkStores<IdentityDbContext>();

            JwtConfiguration authConfiguration = new JwtConfiguration();
            configuration.Bind("JWT", authConfiguration);
            services.AddSingleton(authConfiguration);

            services.AddScoped<AuthenticationService>();

            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
