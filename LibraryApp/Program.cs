using Avalonia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LibraryApp;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .AfterSetup(_ =>
            {
                var services = new ServiceCollection();
                services.AddDbContext<Data.LibraryContext>(options =>
                {
                    var path = System.IO.Path.Combine(AppContext.BaseDirectory, "library.db");
                    options.UseSqlite($"Data Source={path}");
                });

                var provider = services.BuildServiceProvider();
                App.ServiceProvider = provider;
            });
}
