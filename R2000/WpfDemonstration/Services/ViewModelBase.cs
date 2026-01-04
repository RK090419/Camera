using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Interfaces;
using WpfDemonstration.Helpers;
namespace WpfDemonstration.Services;
public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    private CancellationTokenSource? _cts;
    public CancellationToken CancellationToken => _cts?.Token ?? CancellationToken.None;

    //protected readonly ILogger Logger = GlobalLogger.Logger;

    public virtual Task Loaded()
    {
        UIHelpers.EnsureUIThread();

        Debug.Assert(_cts is null);
        _cts = new();

        return Task.CompletedTask;
    }

    public virtual void UnLoaded()
    {
        UIHelpers.EnsureUIThread();
        if (_cts is not null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }


}