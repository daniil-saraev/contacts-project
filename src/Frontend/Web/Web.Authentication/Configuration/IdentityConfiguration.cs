using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Identity.Common.Data;
using Identity.Common.Models;
using Identity.Common.Services;
using Identity.Common.Configuration;

namespace Web.Authentication.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserDbConnection"));
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                IdentityOptionsConfiguration.ConfigureDefaultIdentityOptions(options);

            }).AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultUI();

            JwtConfiguration authConfiguration = new JwtConfiguration();
            configuration.Bind("JWT", authConfiguration);
            services.AddSingleton(authConfiguration);

            services.AddScoped<RefreshTokenFilter>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
