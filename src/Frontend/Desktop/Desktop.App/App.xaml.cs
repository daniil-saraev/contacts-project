global using Domain.Core.Entities;
global using ApiServices.Identity;
global using ApiServices.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Desktop.Services.Factories;
using System.Drawing;

namespace Desktop.App
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

            await _host.Services.GetRequiredService<MainViewModel>().InitializeAsync();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            _host.Services.GetRequiredService<INavigationService>().NavigateTo<HomeViewModel>();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
