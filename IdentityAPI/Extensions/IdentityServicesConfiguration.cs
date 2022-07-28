using Core.Models.Identity;
using IdentityAPI.Data;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Extensions
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

            //var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
              //  .AddConfigurationStore(options =>
              //{
              //    options.ConfigureDbContext = b =>
              //        b.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"),
              //            sql => sql.MigrationsAssembly(migrationsAssembly));
              //}).AddOperationalStore(options =>
              //{
              //    options.ConfigureDbContext = b =>
              //        b.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"),
              //            sql => sql.MigrationsAssembly(migrationsAssembly));
              //})
              .AddAspNetIdentity<ApplicationUser>()
              .AddInMemoryApiResources(Configuration.ApiResources)
              .AddInMemoryIdentityResources(Configuration.IdentityResources)
              .AddInMemoryApiScopes(Configuration.ApiScopes)
              .AddInMemoryClients(Configuration.Clients)
              .AddProfileService<ProfileService>()
              .AddDeveloperSigningCredential();

            return services;
        }
    }
}
