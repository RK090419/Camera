using System.Windows;

namespace R2000Wpf.Interfaces;
public interface IContentChangingControl
{
    void SetContent<T>() where T : UIElement;
}
