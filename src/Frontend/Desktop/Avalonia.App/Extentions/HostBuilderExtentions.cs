using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;
using DesktopApp.Services.Authentication.TokenProvider;
using DesktopApp.Services.ExceptionHandler;
using DesktopApp.Services.Authentication;
using DesktopApp.ViewModels.Account;
using DesktopApp.Services.Data.Persistence.RemoteRepositoryProvider;
using DesktopApp.Commands.Account.RestoreCommand;
using DesktopApp.Services.Navigation;
using DesktopApp.Services.Authentication.TokenDecoder;
using DesktopApp.Commands.Contacts.LoadCommand;
using DesktopApp.Interactors;
using DesktopApp.ViewModels.Contacts;
using DesktopApp.ViewModels;
using DesktopApp.Services.Factories;
using DesktopApp.Services.Authentication.TokenStorage;
using DesktopApp.Services.Data.Persistence.DiskProvider;
using DesktopApp.Services.Data.FileServices;
using DesktopApp.Services.Data.UnitOfWork;
using DesktopApp.Services.Authentication.HttpClientHandlers;
using DesktopApp.Views;
using System;
using Api.Services.Gateway.Interfaces;
using Api.Services.Gateway.Services;
using Api.Services.Gateway.Identity;
using Core.Entities;

namespace Avalonia.App.Extentions
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
                services.AddHttpClient<IRepository<Contact>, ContactsApiService>()
                        .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

                services.AddSingleton(typeof(IFileService<UnitOfWorkState<Contact>>), new JsonFileService<UnitOfWorkState<Contact>>($"{Environment.CurrentDirectory}\\contacts.json"));
                services.AddSingleton(typeof(IFileService<TokenResponse>), new JsonFileService<TokenResponse>($"{Environment.CurrentDirectory}\\user.json"));

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