using System;
using Avalonia.Threading;
using AvaloniaDemonstration.Controls;
using Core.Interfaces;

namespace AvaloniaDemonstration.Services;
public partial class NavigationService : INavigationService
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
        Dispatcher.UIThread.VerifyAccess();

        var dst = _viewProvider().GetView<TTo>(navigationKey);

        var dstViewModel = _viewProvider().GetViewModel(dst.ViewModelType);

        dst.SetViewModel(dstViewModel);

        _frame.Navigate(dst);
        return true;
    }

    public bool Navigate<TFrom, TTo>(object? navigationParam = null) where TFrom : IViewBase where TTo : IViewBase
    {
        Dispatcher.UIThread.VerifyAccess();

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