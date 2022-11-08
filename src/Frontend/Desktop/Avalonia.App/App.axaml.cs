using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.App.Extentions;

namespace Avalonia.App
{
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

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            _host.Start();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _host.Services.GetRequiredService<MainViewModel>().InitializeAsync();
                desktop.MainWindow = _host.Services.GetRequiredService<MainWindow>();
                _host.Services.GetRequiredService<INavigationService>().NavigateTo<HomeViewModel>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
