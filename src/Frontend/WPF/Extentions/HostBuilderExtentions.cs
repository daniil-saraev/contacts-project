using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Desktop.Services.Authentication;
using ApiServices.Services;
using Desktop.Services.Factories;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.UnitOfWork;
using Desktop.Interactors;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Commands.Contacts.LoadCommand;
using Desktop.ViewModels;
using Desktop.ViewModels.Contacts;
using Desktop.ViewModels.Account;
using Desktop.Services.Navigation;
using Core.Interfaces;
using Desktop.Commands.Account.RestoreCommand;
using Desktop.Services.Authentication.HttpClientHandlers;
using Desktop.Services.ExceptionHandler;
using ApiServices.Interfaces;
using ApiServices.Identity;

namespace Desktop.Extentions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder RegisterServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                services.AddSingleton<IIdentityApi, IdentityApiService>();
                services.AddSingleton<ITokenStorage, TokenStorage>();
                services.AddSingleton<ITokenDecoder, TokenDecoder>();
                services.AddSingleton<ITokenValidator, TokenValidator>();
                services.AddSingleton<TokenProvider>();

                services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
			    services.AddHttpClient<IRepository<Contact>, ContactsDatabaseApiService>()
                	    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

                services.AddSingleton(typeof(IFileService<UnitOfWorkState<Contact>>), new JsonFileService<UnitOfWorkState<Contact>>("contacts.json"));
                services.AddSingleton(typeof(IFileService<TokenResponse>), new JsonFileService<TokenResponse>("user.json"));

                services.AddSingleton<IExceptionHandler, MessageBoxExceptionHandler>();   
                services.AddSingleton<UnitOfWork<Contact>>();
                services.AddSingleton<IDiskProvider, DiskProvider>();
                services.AddSingleton<IRemoteRepositoryProvider, RemoteRepositoryProvider>();
                services.AddSingleton<PersistenceProvidersFactory>();
                services.AddSingleton<IViewModelsFactory, ViewModelsFactory>();
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<SelectedContact>();
                services.AddSingleton<ContactsInteractor>();
                services.AddSingleton<IContactsStore>((services) => services.GetRequiredService<ContactsInteractor>());
                services.AddSingleton<IContactsPresenter>((services) => services.GetRequiredService<ContactsInteractor>());
                    
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
            });

            return hostBuilder;
        } 

        public static IHostBuilder RegisterCommands(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) => 
            {
                services.AddSingleton<ILoadContactsCommand, LoadContactsCommand>();
                services.AddSingleton<IRestoreSessionCommand, RestoreSessionCommand>();
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