using System;
using Avalonia;
using AvaloniaDemonstration.Helpers;
using AvaloniaDemonstration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaDemonstration;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = App.CreateBuilder(args, BuildAvaloniaApp);

        //builder.Services.AddSingleton<IViewProvider, ViewProvider>();
        //builder.Services.AddSingleton<INavigationService>(x =>
        //{
        //    return new NavigationService(() => x.GetRequiredService<IViewProvider>(), App.Current.GetVisualInstance<MainWindow>().PageFrame);
        //});

        builder.Services.AddSingleton<MainView>();
        builder.Services.AddSingleton<CameraView>();

        builder.Services.AddSingleton<MainWindowViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<CameraViewModel>();


        App app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        GlobalLogger.Initialize(logger);

        logger.LogInformation("Application starting...");

        app.Run();
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
