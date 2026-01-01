using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Core.Interfaces;

namespace AvaloniaDemonstration.Services;
public abstract class ViewBase : UserControl { }

public abstract class ViewBase<T> : ViewBase, IViewBase where T : IViewModelBase
{
    public object? GetViewModel() => DataContext;

    protected T ViewModel => (T)(DataContext ?? (Design.IsDesignMode ? null! : throw new InvalidOperationException("ViewModel is null")));


    public Type ViewModelType => typeof(T);

    static ViewBase()
    {
        UserControl.UnloadedEvent.AddClassHandler<ViewBase<T>>((view, _) =>
        {
            view.ViewModel?.Unloaded();
        }, RoutingStrategies.Direct);
        UserControl.LoadedEvent.AddClassHandler<ViewBase<T>>((view, _) =>
        {
            view.ViewModel?.Loaded();
        }, RoutingStrategies.Direct);

    }

    public virtual void SetViewModel(IViewModelBase viewModel)
    {
        if (viewModel is not T vm)
        {
            throw new InvalidOperationException("Invalid ViewModel type");
        }

        this.DataContext = vm;
    }
}
