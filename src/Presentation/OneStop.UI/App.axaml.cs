using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneStop.Persistence;
using OneStop.UI.ViewModels;
using OneStop.UI.Views;
using System;
using System.IO;

namespace OneStop.UI;

public partial class App : Application
{
    // Property global untuk menyimpan DI Container
    public new static App? Current => Application.Current as App;
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 1. Setup Configuration (Membaca appsettings.json)
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        // 2. Setup Dependency Injection
        var services = new ServiceCollection();

        // Inject Database (Layer Persistence)
        // Pastikan OneStop.Persistence sudah di-reference di project ini
        services.AddPersistence(configuration);

        // Inject ViewModels
        // Kita pakai MainWindowViewModel sesuai perubahan terakhir
        services.AddTransient<MainWindowViewModel>();

        // 3. Build Provider (Simpan ke property Services)
        Services = services.BuildServiceProvider();

        // 4. Setup Main Window
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Hapus validator bawaan Avalonia agar log bersih
            BindingPlugins.DataValidators.RemoveAt(0);

            // Ambil ViewModel dari DI Container
            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}