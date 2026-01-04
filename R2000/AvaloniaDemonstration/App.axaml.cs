using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using AvaloniaDemonstration.Helpers;
using AvaloniaDemonstration.ViewModels;
using AvaloniaDemonstration.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R86.Avalonia.Hosting;

namespace AvaloniaDemonstration;

public partial class App : HostedApplication<App>
{
    protected readonly ILogger Logger = GlobalLogger.Logger;
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
    public T GetVisualInstance<T>() where T : Visual
    {
        Visual root;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            root = lifetime.MainWindow ?? throw new InvalidOperationException();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime lifetime2)
        {
            root = lifetime2.MainView ?? throw new InvalidOperationException();
        }
        else
        {
            throw new NotSupportedException();
        }



        var r = root.FindDescendantOfType<T>(includeSelf: true)
            ?? root.FindAncestorOfType<T>(includeSelf: true);

        return r ?? throw new InvalidCastException();
    }
    public T GetRequiredService<T>() where T : notnull
    {
        try
        {
            var scopeServiceProvider = CurrentScope?.ServiceProvider;
            if (scopeServiceProvider == null)
            {
                T s = Services.GetRequiredService<T>();
                return s;
            }
            return scopeServiceProvider.GetRequiredService<T>();
        }
        catch (ObjectDisposedException ex)
        {
            try
            {

                Logger.LogError("Failed to resolve type {type} in {stackTrace}", typeof(T), ex.StackTrace);
            }
            catch { }
            throw new ObjectDisposedException($"Failed to resolve type {typeof(T)}.");
        }
        catch (Exception ex)
        {

            try
            {

                Logger.LogError("from App.GetRequiredService, error: {error}, {st}", ex, ex.StackTrace);
            }
            catch { }

            Debugger.Break();
            if (Design.IsDesignMode)
            {
                return default(T)!;
            }
            throw;
        }
    }

    public object GetRequiredService(Type type)
    {
        try
        {
            var scopeServiceProvider = CurrentScope?.ServiceProvider;
            if (scopeServiceProvider == null)
            {
                object s = Services.GetRequiredService(type);
                return s;
            }
            return scopeServiceProvider.GetRequiredService(type);
        }
        catch (ObjectDisposedException ex)
        {
            try
            {

                Logger.LogError("Failed to resolve type {type} in {stackTrace}", type, ex.StackTrace);
            }
            catch { }
            throw new ObjectDisposedException($"Failed to resolve type {type}.");
        }
        catch (Exception ex)
        {

            try
            {

                Logger.LogError("from App.GetRequiredService, error: {error}, {st}", ex, ex.StackTrace);
            }
            catch { }

            Debugger.Break();
            if (Design.IsDesignMode)
            {
                return default!;
            }
            throw;
        }
    }

    public T GetRequiredKeyedService<T>(object? key = null) where T : notnull
    {
        try
        {
            var scopeServiceProvider = CurrentScope?.ServiceProvider;
            if (scopeServiceProvider == null)
            {
                T s = Services.GetRequiredKeyedService<T>(key);
                return s;
            }
            return scopeServiceProvider.GetRequiredKeyedService<T>(key);
        }
        catch (ObjectDisposedException ex)
        {
            try
            {

                Logger.LogError("Failed to resolve type {type} in {stackTrace}", typeof(T), ex.StackTrace);
            }
            catch { }
            throw new ObjectDisposedException($"Failed to resolve type {typeof(T)}.");
        }
        catch (Exception ex)
        {

            try
            {

                Logger.LogError("from App.GetRequiredService, error: {error}, {st}", ex, ex.StackTrace);
            }
            catch { }

            Debugger.Break();
            if (Design.IsDesignMode)
            {
                return default(T)!;
            }
            throw;
        }
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