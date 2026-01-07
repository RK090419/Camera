using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Resources;
using R2000Wpf.Services;

namespace R2000Wpf.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
    }

    [ObservableProperty]
    string? _greeting = MainWindowStrings.Greeting;
    [RelayCommand]
    void ChangeCulture()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("he-IL");
    }
}
