using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Desktop.Main.Common.Services;
using Desktop.Common.Services;
using Desktop.Main.Contacts.Models;
using Desktop.Main.Contacts.ViewModels;
using Desktop.Main.Account.ViewModels;
using Desktop.Main.Common.ViewModels;
using Desktop.Main.Common.Views;
using Desktop.Authentication.Configuration;
using Desktop.Contacts.Configuration;
using Desktop.Main.Contacts.Notifier;

namespace Desktop.Main.Common.Extentions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder RegisterServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IExceptionHandler, MessageBoxExceptionHandler>();
                services.AddSingleton<IViewModelsFactory, ViewModelsFactory>();
                services.AddSingleton<INavigationService>((services) => new NavigationService(services.GetRequiredService<IViewModelsFactory>(),
                                                            services.GetRequiredService<HomeViewModel>()));                                                                                           
                services.AddSingleton<INotifyContactsChanged, ContactsChangesNotifier>();
                services.AddSingleton<SelectedContact>();
                services.RegisterIdentityServices();
                services.RegisterContactsServices();
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