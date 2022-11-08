using ContactBook.Configuration;
using Web.ContactBook.Configuration;

namespace Web.Extensions
{
    public static class CoreServicesConfiguration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {        
            WebContactBookConfiguration.RegisterWebContactBook(configuration, services);

            return services;
        }
    }
}
