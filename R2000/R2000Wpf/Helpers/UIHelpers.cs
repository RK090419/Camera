using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using R2000Wpf.Models;

namespace R2000Wpf.Helpers;

public static class UIHelpers
{
    public static void EnsureUIThread()
    {
        if (!App.Current.Dispatcher.CheckAccess())
            throw new InvalidOperationException("This method must be called on the UI thread.");
    }
    public static IServiceCollection AddViewModels(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            if (typeof(ViewModelBase).IsAssignableFrom(type))
            {
                services.Add(new ServiceDescriptor(type, type, lifetime));
            }
        }

        return services;
    }
    public static IServiceCollection AddViews(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            if (typeof(ViewBase).IsAssignableFrom(type))
            {
                services.Add(new ServiceDescriptor(type, type, lifetime));
            }
        }

        return services;
    }

    public static T GetVisualInstance<T>() where T : Visual
    {
        if (App.Current.MainWindow is not Visual root)
            throw new InvalidOperationException("MainWindow is not set");

        var result = FindDescendantOrSelf<T>(root) ?? FindAncestorOrSelf<T>(root);

        return result ?? throw new InvalidCastException($"Visual of type {typeof(T).Name} not found");
    }

    private static T? FindDescendantOrSelf<T>(DependencyObject parent) where T : Visual
    {
        if (parent is T t) return t;

        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            var result = FindDescendantOrSelf<T>(child);
            if (result != null) return result;
        }

        return null;
    }

    private static T? FindAncestorOrSelf<T>(DependencyObject child) where T : Visual
    {
        while (child != null)
        {
            if (child is T t) return t;
            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }
}
