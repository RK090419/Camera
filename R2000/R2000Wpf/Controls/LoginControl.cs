using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace R2000Wpf.Controls;

public class LoginControl : Control
{
    static LoginControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LoginControl), new FrameworkPropertyMetadata(typeof(LoginControl)));
    }
    public static readonly DependencyProperty LogInCommandProperty = DependencyProperty.Register(
   "LogInCommand", typeof(ICommand),
   typeof(LoginControl)
   );

    public ICommand? LogInCommand
    {
        get => (ICommand?)GetValue(LogInCommandProperty);
        set => SetValue(LogInCommandProperty, value);
    }

}
