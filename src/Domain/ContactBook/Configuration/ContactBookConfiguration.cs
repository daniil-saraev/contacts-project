using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactBook.Configuration;

public static class ContactBookConfiguration
{
    public static void RegisterContactBookService(IConfiguration configuration, IServiceCollection services)
    {
        services.AddScoped<IContactBookService, ContactBookService>();
    }
}