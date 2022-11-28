using Contacts.Common.Data;
using Contacts.Common.Services;
using Core.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediatR;
using Core.Common.Entities;
using Core.Contacts.Models;
using Contacts.Common.Commands;
using Contacts.Common.Queries;
using Core.Contacts.Requests;

namespace Contacts.Common.Configuration;

public static class ContactsServicesConfiguration
{
    public static void RegisterContactsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection"));
        });

        services.AddScoped<IContactsRepository, ContactsRepository>();

        services.AddHttpContextAccessor();
        services.AddSingleton<IUserInfoService, UserInfoService>();

        services.AddMediatR(typeof(GetAllQuery).Assembly);

        services.AddAutoMapper(config =>
        {
            config.CreateProfile(string.Empty, config =>
            {
                config.CreateMap<Contact, ContactData>();
                config.CreateMap<AddContactRequest, CreateRequest>();
                config.CreateMap<UpdateContactRequest, UpdateRequest>();
            });
        });
    }

    public static async Task InitializeDbAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ContactsDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}