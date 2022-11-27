using Api.Services.Gateway.Contacts;
using Core.Contacts.Interfaces;
using Desktop.Authentication.HttpClientHandlers;
using Desktop.Authentication.Models;
using Desktop.Common.Services;
using Desktop.Contacts.Persistence;
using Desktop.Contacts.Services;
using Desktop.Contacts.Services.SyncService;
using Microsoft.Extensions.DependencyInjection;
using static Core.Common.Constants.BaseUrls;

namespace Desktop.Contacts.Configuration;

public static class ContactsServicesConfiguration
{
    public static void RegisterContactsServices(this IServiceCollection services)
    {
        services.AddTransient<AuthenticationHeaderHandler>();
        services.AddHttpClient<ContactBookService>(client => new ContactBookService(client))
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();

        services.AddSingleton<ISyncService, SyncService>(services => new SyncService(services.GetRequiredService<ContactBookService>()));
        services.AddTransient<AuthenticatedContactBookService>();
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