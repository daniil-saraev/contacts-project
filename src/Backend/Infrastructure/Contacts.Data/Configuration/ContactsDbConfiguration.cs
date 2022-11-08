using Contacts.Data.Context;
using Contacts.Data.Services;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Data.Configuration;

public static class ContactsDbConfiguration
{
    public static void RegisterRepository(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection"));
        });

        services.AddScoped<IContactsRepository, ContactsRepository>();
    }

    public static async Task InitializeDbAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ContactsDbContext>();
        //await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}