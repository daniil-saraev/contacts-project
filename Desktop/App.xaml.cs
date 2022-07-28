using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Desktop.Data;
using Core.Interfaces;
using Desktop.ViewModels;
using ApiServices;
using Desktop.Services;
using Desktop.ViewModels.Contacts;
using Microsoft.EntityFrameworkCore;

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
                    //services.AddSingleton(typeof(IRepository<Contact>),new ContactsDbApiService(BaseUrls.ContactsWebApiUrl, new HttpClient()));              
                    services.AddSingleton<TestDbContext>();
                    services.AddSingleton<IRepository<Contact>, TestRepository>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            using (var context = _host.Services.GetRequiredService<TestDbContext>())
            {
                context.Database.EnsureCreated();
            }

            RepositoryService.Initialize(_host.Services.GetRequiredService<IRepository<Contact>>());

            NavigationService.GetNavigationService().SetCurrentViewModel = new HomeViewModel();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected async override void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
