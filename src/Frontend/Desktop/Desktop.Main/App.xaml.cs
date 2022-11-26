using System.Windows;
using Desktop.Common.Commands;
using Desktop.Main.Common.Extentions;
using Desktop.Main.Common.ViewModels;
using Desktop.Main.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Desktop.Main
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
                .RegisterViewModels()
                .RegisterViews()
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            
            BaseCommand.SetServiceProvider(_host.Services);

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            _host.Services.GetRequiredService<MainViewModel>().OnStartup();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
