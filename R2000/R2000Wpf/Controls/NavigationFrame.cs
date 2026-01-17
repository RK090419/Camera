using System.Windows.Controls;
using Core.Interfaces;
using R2000Wpf.Helpers;

namespace R2000Wpf.Controls;

public class NavigationFrame : ContentControl
{
    public void Navigate(IViewBase newContent)
    {
        UIHelpers.EnsureUIThread();

        Content = newContent;
    }
}
