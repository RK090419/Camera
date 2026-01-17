using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace R2000Wpf.Resources;

public class LocalizedStrings : INotifyPropertyChanged
{
    public static LocalizedStrings Instance { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ChangeCulture(CultureInfo culture)
    {
        if (culture == null) throw new ArgumentNullException(nameof(culture));

        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;

        OnPropertyChanged(string.Empty);
    }

    public MainWindowStrings MainWindowStrings => new();
    public LoginControlStrings LoginControlStrings => new();
    public UserViewStrings UserViewStrings => new();

    public string GetString(Func<object> resourceProperty)
    {
        return resourceProperty()?.ToString() ?? string.Empty;
    }
}