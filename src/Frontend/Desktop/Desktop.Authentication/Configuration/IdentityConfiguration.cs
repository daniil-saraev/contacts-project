using Api.Services.Gateway.Identity;
using Core.Identity.Interfaces;
using Desktop.Authentication.Models;
using Desktop.Authentication.Services;
using Desktop.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using static Desktop.Authentication.Models.User;
using static Core.Common.Constants.BaseUrls;


namespace Desktop.Authentication.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services)
        {
            services.AddSingleton<User>();
            services.AddHttpClient();
            services.AddSingleton<IIdentityService, IdentityService>(services => new IdentityService(IDENTITY_API_URL, services.GetRequiredService<HttpClient>()));
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton(typeof(IFileService<UserData?>), new JsonFileService<UserData?>($"{Environment.CurrentDirectory}\\user.json"));
            services.AddSingleton<IUserDataStorage, UserDataStorage>();

            return services;
        }
    }
}
