using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Interfaces;

namespace R2000Wpf.Controls;

public class LoginControl : Control
{
    public IContentChangingControl _contentHost;
    static LoginControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LoginControl), new FrameworkPropertyMetadata(typeof(LoginControl)));
    }
    public LoginControl()
    {
        SignUpCommand = new RelayCommand(() =>
        {
            if (_contentHost != null)
            {
                var signUp = new SignUpControl();
                signUp.ContentHost = _contentHost; // inject host
                _contentHost.SetContent(signUp);
            }
        });
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

    public ICommand? SignUpCommand { get; }

}
