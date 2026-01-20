using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneStop.Infrastructure; // Namespace untuk AddInfrastructure
using OneStop.Persistence;    // Namespace untuk AddPersistence
using OneStop.UI.ViewModels;
using OneStop.UI.Views;
using System;
using System.IO;

namespace OneStop.UI;

// PERBAIKAN: Menggunakan 'Avalonia.Application' secara eksplisit
// untuk menghindari bentrok dengan namespace 'OneStop.Application'
public partial class App : Avalonia.Application
{
    // Property helper untuk akses global (jika dibutuhkan)
    public new static App? Current => Avalonia.Application.Current as App;

    // Property untuk menyimpan Dependency Injection Container
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

        // 2. Setup Service Collection (Wadah Dependency Injection)
        var services = new ServiceCollection();

        // -> Inject Database (Layer Persistence)
        services.AddPersistence(configuration);

        // -> Inject Services Backend (Layer Infrastructure)
        services.AddInfrastructure();

        // -> Inject ViewModels (Layer Presentation)
        services.AddTransient<MainWindowViewModel>();

        // 3. Build Service Provider (Kunci konfigurasi)
        Services = services.BuildServiceProvider();

        // 4. Setup Main Window
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Menghapus validasi data bawaan Avalonia (opsional, agar log lebih bersih)
            BindingPlugins.DataValidators.RemoveAt(0);

            // Ambil ViewModel dari DI Container (Otomatis inject semua dependency)
            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}