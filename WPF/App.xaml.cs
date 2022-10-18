global using Contact = OpenApi.Contact;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Desktop.ViewModels;
using Desktop.Services.Factories;
using Desktop.Services.Navigation;
using Desktop.Extentions;
using Desktop.ViewModels.Contacts;

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
                .RegisterServices()
                .RegisterCommands()
                .RegisterViewModels()
                .RegisterViews()
                .Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            NavigationService.GetInstance().CurrentViewModel = _host.Services.GetRequiredService<HomeViewModel>();

            await _host.Services.GetRequiredService<MainViewModel>().InitializeAsync();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }       
    }
}
