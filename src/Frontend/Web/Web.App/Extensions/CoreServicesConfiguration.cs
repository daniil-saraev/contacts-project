using Api.Services.Gateway.Contacts;
using Core.Contacts.Interfaces;
using Web.Authentication;
using Web.Authentication.Configuration;

namespace Web.App.Extensions
{
    internal static class CoreServicesConfiguration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<AuthenticationHeaderHandler>();
            services.AddHttpClient<IContactBookService, ContactBookService>(client => new ContactBookService(client))
                    .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.RegisterIdentityServices(configuration);
            return services;
        }
    }
}
