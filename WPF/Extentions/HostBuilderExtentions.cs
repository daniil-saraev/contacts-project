using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Core.Constants;
using Desktop.Services.Authentication;
using ApiServices.Services;
using Desktop.Services.Authentication.TokenServices;
using Desktop.Services.Authentication.UserServices;
using ApiServices.Interfaces;
using Desktop.Services.Factories;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.UnitOfWork;
using Desktop.Services.Authentication.Identity;
using Desktop.Containers;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Services.Containers;
using Desktop.Commands.Contacts.SaveCommand;
using Desktop.Commands.Contacts.LoadCommand;
using Desktop.ViewModels;
using Desktop.ViewModels.Contacts;
using Desktop.ViewModels.Account;

namespace Desktop.Extentions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder RegisterServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                ContactsDatabaseApiService contactsDbApi = new ContactsDatabaseApiService(BaseUrls.CONTACTS_DATABASE_API_URL);
                services.AddSingleton<IRepository<Contact>>(contactsDbApi);
                services.AddSingleton<IApiService>(contactsDbApi);

                IdentityApiService identityApi = new IdentityApiService(BaseUrls.IDENTITY_API_URL);
                services.AddSingleton<IIdentityApi>(identityApi);
                services.AddSingleton<IApiService>(identityApi);

                services.AddSingleton(typeof(IFileService<UnitOfWorkState<Contact>>), new JsonFileService<UnitOfWorkState<Contact>>("contacts.json"));
                services.AddSingleton(typeof(IFileService<UserData>), new JsonFileService<UserData>("user.json"));

                services.AddSingleton<ITokenDecoder, TokenDecoder>();
                services.AddSingleton<IIdentityProvider, IdentityProvider>();            
                services.AddSingleton<UnitOfWork<Contact>>();
                services.AddSingleton<IDiskProvider, DiskProvider>();
                services.AddSingleton<IRemoteRepositoryProvider, RemoteRepositoryProvider>();
                services.AddSingleton<PersistenceProvidersFactory>();
                services.AddSingleton<IViewModelsFactory, ViewModelsFactory>();
                services.AddSingleton<SelectedContact>();

                services.AddSingleton<ContactsContainer>();
                services.AddSingleton<IContactsStore>((services) => services.GetRequiredService<ContactsContainer>());
                services.AddSingleton<IContactsPresenter>((services) => services.GetRequiredService<ContactsContainer>());
                    
                services.AddSingleton<User>();
                services.AddSingleton<AuthenticationService>();
            });

            return hostBuilder;
        } 

        public static IHostBuilder RegisterCommands(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                services.AddSingleton<ISaveCommand, SaveContactsCommand>();
                services.AddSingleton<ILoadCommand, LoadContactsCommand>();
            });
            
            return hostBuilder;
        } 

        public static IHostBuilder RegisterViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                services.AddTransient<ContactAddViewModel>();
                services.AddTransient<ContactEditViewModel>();
                services.AddTransient<ContactInfoViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<AccountViewModel>();
                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();

                services.AddSingleton<MainViewModel>();
            });
            
            return hostBuilder;
        }

        public static IHostBuilder RegisterViews(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                services.AddSingleton<MainWindow>();
            });

            return hostBuilder;
        }
    }
}