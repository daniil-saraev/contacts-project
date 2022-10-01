global using Contact = DatabaseApi.Contact;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Desktop.ViewModels.Contacts;
using Desktop.ViewModels;
using Core.Constants;
using Desktop.Services.ExceptionHandlers;
using Desktop.Services.Navigation;
using Desktop.Stores;
using Desktop.Services.DataServices.FileServices;
using Desktop.Services.Authentication;
using ApiServices.Services;
using Desktop.Services.Authentication.TokenServices;
using Desktop.Services.Authentication.UserServices;
using ApiServices.Interfaces;
using Desktop.Services.DataServices.UnitOfWork;
using Desktop.Factories;
using System.Collections.Generic;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ContactsDatabaseService contactsDbApi = new ContactsDatabaseService(BaseUrls.ContactsDatabaseApiUrl);
                    services.AddSingleton<IRepository<Contact>>(contactsDbApi);
                    services.AddSingleton<IApiService>(contactsDbApi);

                    IdentityService identityApi = new IdentityService(BaseUrls.IdentityApiUrl);
                    services.AddSingleton<IIdentityService>(identityApi);
                    services.AddSingleton<IApiService>(identityApi);

                    services.AddSingleton(typeof(IFileService<UnitOfWork<Contact>>), new JsonFileService<UnitOfWork<Contact>>("contacts.json"));
                    services.AddSingleton(typeof(IFileService<UserData>), new JsonFileService<UserData>("user.json"));

                    services.AddSingleton<ITokenDecoder, TokenDecoder>();
                    services.AddSingleton<IExceptionHandler, DefaultExceptionHandler>();
                    services.AddSingleton(new UnitOfWork<Contact>(new List<Contact>()));
                    services.AddSingleton<IContactsStore, ContactsStore>((serviceProvider) => ContactsStore.GetInstance());
                    services.AddSingleton<SelectedContact>();
                    services.AddSingleton<ViewModelsFactory>();
                    services.AddSingleton<PersistenceProviderFactory>();
                    services.AddSingleton<AuthenticationService>();
                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            ContactsStore.InitializeStore(_host.Services.GetRequiredService<PersistenceProviderFactory>());
            await ContactsStore.GetInstance().LoadContactsAsync();
            NavigationService.InitializeNavigationService(_host.Services.GetRequiredService<ViewModelsFactory>());
            NavigationService.GetNavigationService().CurrentViewModel = new HomeViewModel(_host.Services.GetRequiredService<IContactsStore>(), _host.Services.GetRequiredService<SelectedContact>());

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected async override void OnExit(ExitEventArgs e)
        {
            await ContactsStore.GetInstance().SaveContactsAsync();
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
