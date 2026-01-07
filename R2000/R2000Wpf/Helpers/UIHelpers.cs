using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using R2000Wpf.Services;

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
}
