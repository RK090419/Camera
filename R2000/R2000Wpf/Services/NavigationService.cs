using Core.Interfaces;
using R2000Wpf.Controls;
using R2000Wpf.Helpers;

namespace R2000Wpf.Services;

public sealed partial class NavigationService : INavigationService
{
    readonly NavigationFrame _frame;
    readonly Func<IViewProvider> _viewProvider;

    public NavigationService(Func<IViewProvider> pageProvider, NavigationFrame frame)
    {
        _viewProvider = pageProvider;
        _frame = frame;
    }

    public bool Navigate<TTo>(object? navigationKey = null) where TTo : IViewBase
    {
        UIHelpers.EnsureUIThread();

        var dst = _viewProvider().GetView<TTo>(navigationKey);

        var dstViewModel = _viewProvider().GetViewModel(dst.ViewModelType);

        dst.SetViewModel(dstViewModel);

        _frame.Navigate(dst);
        return true;
    }

    public bool Navigate<TFrom, TTo>(object? navigationParam = null) where TFrom : IViewBase where TTo : IViewBase
    {
        UIHelpers.EnsureUIThread();

        if (_frame.Content is not TFrom src)
        {
            return false;
        }

        var dst = _viewProvider().GetView<TTo>(navigationParam);

        var dstViewModel = _viewProvider().GetViewModel(dst.ViewModelType);


        dst.SetViewModel(dstViewModel);

        _frame.Navigate(dst);
        return true;
    }
}