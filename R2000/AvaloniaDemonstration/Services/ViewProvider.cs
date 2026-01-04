using System;
using System.Diagnostics;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaDemonstration.Services;

public sealed class ViewProvider : IViewProvider
{
    readonly ILogger<ViewProvider> _logger;
    public ViewProvider(ILogger<ViewProvider> logger)
    {
        _logger = logger;
    }

    public IViewBase GetView<T>(object? navigationKey = null) where T : IViewBase
    {
        _logger.LogDebug("Resolving view: {type}, key: {key}", typeof(T), navigationKey ?? "null");

        if (navigationKey is not null)
        {
            return App.Current.GetRequiredKeyedService<T>(navigationKey);
        }
        return App.Current.GetRequiredService<T>();

    }

    public IViewBase? GetView(Type viewType, object? navigationKey = null)
    {
        _logger.LogDebug("Resolving view: {type}, key: {key}", viewType.GetType(), navigationKey ?? "null");

        var provider = App.Current.CurrentScope?.ServiceProvider;
        if (navigationKey is not null && provider is IKeyedServiceProvider kp)
        {
            return kp.GetKeyedService(viewType, navigationKey) as IViewBase;
        }

        return App.Current.GetRequiredService(viewType) as IViewBase;
    }

    public IViewModelBase GetViewModel(Type viewModelType)
    {
        _logger.LogDebug("Resolving view model: {type}", viewModelType);

        var vm = App.Current.GetRequiredService(viewModelType);

        Debug.Assert(vm is IViewModelBase);

        return (IViewModelBase)vm;
    }
}
