using ContactBook.Commands;
using ContactBook.Configuration;
using ContactBook.Queries;
using Contacts.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.ContactBook.Commands;
using Web.ContactBook.Queries;

namespace Web.ContactBook.Configuration;

public static class WebContactBookConfiguration
{
    public static void RegisterWebContactBook(IConfiguration configuration, IServiceCollection services)
    {
        ContactBookConfiguration.RegisterContactBookService(configuration, services);

        services.AddTransient<IAddContactCommand, AddContactCommand>();
        services.AddTransient<IDeleteContactCommand, DeleteContactCommand>();
        services.AddTransient<IUpdateContactCommand, UpdateContactCommand>();
        services.AddTransient<IGetContactsQuery, GetContactsQuery>();

        ContactsDbConfiguration.RegisterRepository(configuration, services);
    }
}