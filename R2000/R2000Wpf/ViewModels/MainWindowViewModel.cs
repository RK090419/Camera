using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Resources;
using R2000Wpf.Services;

namespace R2000Wpf.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        LocalizedStrings.Instance.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(string.Empty);
        };
    }

    public string Greeting => MainWindowStrings.Greeting;

    [RelayCommand]
    void ChangeCulture()
    {
        var defaultCulture = new CultureInfo("en-US");
        var hebrewCulture = new CultureInfo("he-IL");

        var current = CultureInfo.CurrentUICulture;

        LocalizedStrings.Instance.ChangeCulture(current.Name == defaultCulture.Name ?
            hebrewCulture : defaultCulture);

    }
}
