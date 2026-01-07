namespace R2000Wpf.Helpers;

public static class UIHelpers
{
    public static void EnsureUIThread()
    {
        if (!App.Current.Dispatcher.CheckAccess())
            throw new InvalidOperationException("This method must be called on the UI thread.");
    }
}
