using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using R2000Wpf.Helpers;
using R2000Wpf.Services;
using R2000Wpf.Views;
namespace R2000Wpf;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new App();

        var builder = App.CreateBuilder();

        builder.Services
         .AddViewModels(typeof(App).Assembly)
         .AddViews(typeof(App).Assembly);
        builder.Services.AddSingleton<MainWindow>();

        builder.Services.AddSingleton<IViewProvider, ViewProvider>();
        builder.Services.AddSingleton<INavigationService>(x =>
        {
            return new NavigationService(() => x.GetRequiredService<IViewProvider>(), UIHelpers.GetVisualInstance<MainWindow>().NavigationWindow);
        });

        app = (App)builder.Build();
        app.InitLogger();

        var mainWindow = app.Services!.GetRequiredService<MainWindow>();

        app.RunWithStartingWindow(mainWindow);
    }
}
