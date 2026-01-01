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

    public event EventHandler? CanGoBackChanged;

    public void ClearNavigationHistory()
    {
        _frame.ClearHistory();

        CanGoBackChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool Navigate(Type dst)
    {
        Dispatcher.UIThread.VerifyAccess();
        App.Current.DisposeScope();

        var view = _viewProvider().GetView(dst);

        if (view is not IViewBase vb)
        {
            return false;
        }

        var dstViewModel = _viewProvider().GetViewModel(vb.ViewModelType);

        vb.SetViewModel(dstViewModel);

        _frame.Navigate(vb);

        CanGoBackChanged?.Invoke(this, EventArgs.Empty);

        return true;
    }

    public void Clear()
    {
        if (_frame.Content is not null)
        {
            _frame.Navigate(null);

            CanGoBackChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool Navigate<TTo>(object? navigationKey = null) where TTo : IViewBase
    {
        Dispatcher.UIThread.VerifyAccess();

        var dst = _viewProvider().GetView<TTo>(navigationKey);

        var dstViewModel = _viewProvider().GetViewModel(dst.ViewModelType);

        dst.SetViewModel(dstViewModel);

        _frame.Navigate(dst);

        CanGoBackChanged?.Invoke(this, EventArgs.Empty);

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

        CanGoBackChanged?.Invoke(this, EventArgs.Empty);

        return true;
    }
}