global using Contact = OpenApi.Contact;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Desktop.ViewModels.Contacts;
using Desktop.ViewModels;
using Core.Constants;
using Desktop.Services.ExceptionHandlers;
using Desktop.Services.Navigation;
using Desktop.Services.Authentication;
using ApiServices.Services;
using Desktop.Services.Authentication.TokenServices;
using Desktop.Services.Authentication.UserServices;
using ApiServices.Interfaces;
using Desktop.Services.Data;
using Desktop.Services.Factories;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.UnitOfWork;
using Desktop.Services.Authentication.Identity;
using Desktop.Containers;

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
                    ContactsDatabaseApiService contactsDbApi = new ContactsDatabaseApiService(BaseUrls.CONTACTS_DATABASE_API_URL);
                    services.AddSingleton<IRepository<Contact>>(contactsDbApi);
                    services.AddSingleton<IApiService>(contactsDbApi);

                    IdentityApiService identityApi = new IdentityApiService(BaseUrls.IDENTITY_API_URL);
                    services.AddSingleton<IIdentityApi>(identityApi);
                    services.AddSingleton<IApiService>(identityApi);

                    services.AddSingleton(typeof(IFileService<UnitOfWorkState<Contact>>), new JsonFileService<UnitOfWorkState<Contact>>("contacts.json"));
                    services.AddSingleton(typeof(IFileService<UserData>), new JsonFileService<UserData>("user.json"));

                    services.AddSingleton<ITokenDecoder, TokenDecoder>();
                    services.AddSingleton<IExceptionHandler, DefaultExceptionHandler>();
                    services.AddSingleton<IIdentityProvider, IdentityProvider>();
                    services.AddSingleton<User>();
                    services.AddSingleton<UnitOfWork<Contact>>();                
                    services.AddSingleton<SelectedContact>();                         
                    services.AddSingleton<PersistenceProviderFactory>();
                    services.AddSingleton<IContactsStore, ContactsStore>();
                    services.AddSingleton<ContactsStore>();
                    services.AddSingleton<ContactCommandsFactory>();
                    services.AddSingleton<ViewModelsFactory>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<AuthenticationService>();
                    services.AddSingleton<AccountCommandsFactory>();
                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                }).Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var authenticationService = _host.Services.GetRequiredService<AuthenticationService>();
            await authenticationService.RestoreSessionAsync();

            var store = _host.Services.GetRequiredService<ContactsStore>();
            await store.LoadContactsAsync();

            NavigationService.InitializeNavigationService(_host.Services.GetRequiredService<ViewModelsFactory>());
            NavigationService.GetNavigationService().CurrentViewModel = new HomeViewModel(
                _host.Services.GetRequiredService<IContactsStore>(), 
                _host.Services.GetRequiredService<SelectedContact>(),
                _host.Services.GetRequiredService<ContactCommandsFactory>());

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var store = _host.Services.GetRequiredService<ContactsStore>();
            store.SaveContacts().Wait();
            base.OnExit(e);
        }
    }
}
