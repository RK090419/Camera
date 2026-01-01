using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaDemonstration.ViewModels;
using AvaloniaDemonstration.Views;
using Microsoft.Extensions.DependencyInjection;
using R86.Avalonia.Hosting;

namespace AvaloniaDemonstration;

public partial class App : HostedApplication<App>
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public IServiceScope? CurrentScope { get; private set; }

    public void CreateScope()
    {
        App.Current.CurrentScope = Services.CreateScope();
    }

    internal void DisposeScope()
    {
        App.Current.CurrentScope?.Dispose();
        App.Current.CurrentScope = null;
    }
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}