using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaDemonstration.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AvaloniaDemonstration.ViewModels;


public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    static readonly CancellationToken CanceledToken = new CancellationToken(true);
    CancellationTokenSource? _tokenSource;
    protected readonly ILogger Logger;
    protected ViewModelBase()
    {
        Logger = GlobalLogger.Logger;
    }

    public virtual Task Loaded()
    {
        Dispatcher.UIThread.VerifyAccess();

        Debug.Assert(_tokenSource is null);

        _tokenSource = new();

        return Task.CompletedTask;
    }

    public virtual void UnLoaded()
    {
        Dispatcher.UIThread.VerifyAccess();

        if (_tokenSource != null)
        {
            try
            {
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel();
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception occurred: {ex}", ex);
            }
            finally
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }
    }


    public virtual void ReNavigated()
    {
        Dispatcher.UIThread.VerifyAccess();

        Debug.Assert(_tokenSource is null);

        _tokenSource = new();
    }

    public void LifetimeEnd() => UnLoaded();
    public void LifetimeBegin()
    {
        _ = Loaded();
    }

    public CancellationToken CancellationToken => _tokenSource?.Token ?? CanceledToken;

}
