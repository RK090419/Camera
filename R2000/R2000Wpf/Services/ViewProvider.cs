using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace R2000Wpf.Services;

public sealed partial class ViewProvider : IViewProvider
{
    readonly ILogger<ViewProvider> _logger;
    public ViewProvider(ILogger<ViewProvider> logger)
    {
        _logger = logger;
    }
    public IViewBase GetView<T>(object? navigationParam = null) where T : IViewBase
    {
        _logger.LogDebug("Resolving view: {type}, key: {key}", typeof(T), navigationParam ?? "null");
        if (navigationParam is not null)
        {
            return App.Current.Services!.GetRequiredKeyedService<T>(navigationParam);
        }
        return App.Current.Services!.GetRequiredService<T>();
    }

    public IViewBase? GetView(Type viewType, object? navigationParam = null)
    {
        _logger.LogDebug("Resolving view: {type}, key: {key}", viewType.GetType(), navigationParam ?? "null");
        var provider = App.Current.CurrentScope?.ServiceProvider;

        if (navigationParam is not null && provider is IKeyedServiceProvider kp)
        {
            return kp.GetKeyedService(viewType, navigationParam) as IViewBase;
        }

        return App.Current.Services!.GetRequiredService(viewType) as IViewBase;
    }

    public IViewModelBase GetViewModel(Type viewModelType)
    {
        _logger.LogDebug("Resolving view model: {type}", viewModelType);
        var vm = App.Current.Services!.GetRequiredService(viewModelType) as IViewModelBase;

        if (vm is null)
            throw new InvalidOperationException($"{viewModelType} does not implement IViewModelBase");

        return (IViewModelBase)vm;
    }
}