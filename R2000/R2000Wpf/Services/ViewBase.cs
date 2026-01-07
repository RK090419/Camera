using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Core.Interfaces;

namespace R2000Wpf.Services;
public abstract class ViewBase : UserControl { }

public abstract class ViewBase<T> : ViewBase, IViewBase where T : IViewModelBase
{

    public object? GetViewModel() => DataContext;

    protected T ViewModel => (T)(DataContext ?? (DesignerProperties.GetIsInDesignMode(new DependencyObject()) ? null! : throw new InvalidOperationException("ViewModel is null")));

    public Type ViewModelType => typeof(T);

    static ViewBase()
    {
        EventManager.RegisterClassHandler(typeof(ViewBase<>),
        LoadedEvent, new RoutedEventHandler((sender, e) =>
        {
            if (sender is ViewBase<T> view)
                view.ViewModel?.Loaded();
        }));

        EventManager.RegisterClassHandler(typeof(ViewBase<>),
        UnloadedEvent, new RoutedEventHandler((sender, e) =>
        {
            if (sender is ViewBase<T> view)
                view.ViewModel?.Unloaded();
        }));
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