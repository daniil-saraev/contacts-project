using Api.Services.Gateway.Contacts;
using Core.Contacts.Interfaces;
using Desktop.Authentication.HttpClientHandlers;
using Desktop.Authentication.Models;
using Desktop.Common.Services;
using Desktop.Contacts.Persistence;
using Desktop.Contacts.Services;
using Microsoft.Extensions.DependencyInjection;
using static Core.Common.Constants.BaseUrls;

namespace Desktop.Contacts.Configuration;

public static class ContactsServicesConfiguration
{
    public static void RegisterContactsServices(this IServiceCollection services)
    {
        services.AddTransient<AuthenticationHeaderHandler>();
        services.AddHttpClient<ContactBookService>(client => new ContactBookService(CONTACTS_DATABASE_API_URL, client))
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();

        services.AddTransient((services) => 
            new AuthenticatedContactBookService(services.GetRequiredService<ContactBookService>(),
            services.GetRequiredService<ILocalContactsStorage>(), services.GetRequiredService<ContactsUnitOfWork>()));

        services.AddTransient<NotAuthenticatedContactBookService>();

        services.AddSingleton(typeof(IFileService<UnitOfWorkState?>), new JsonFileService<UnitOfWorkState?>($"{Environment.CurrentDirectory}\\contacts.json"));
        services.AddSingleton<ILocalContactsStorage, LocalContactsStorage>();
        services.AddSingleton<ContactsUnitOfWork>();
        
        services.AddTransient<IContactBookService>((services) => services.GetRequiredService<User>().IsAuthenticated 
                                                                                    ? services.GetRequiredService<AuthenticatedContactBookService>()
                                                                                    : services.GetRequiredService<NotAuthenticatedContactBookService>());

        services.AddTransient<IPersistenceProvider>((services) => services.GetRequiredService<User>().IsAuthenticated 
                                                                                    ? services.GetRequiredService<AuthenticatedContactBookService>()
                                                                                    : services.GetRequiredService<NotAuthenticatedContactBookService>());
    }
}