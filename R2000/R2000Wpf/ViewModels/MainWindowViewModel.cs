using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using R2000Wpf.Models;
using R2000Wpf.Resources;

namespace R2000Wpf.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    readonly ILogger<MainWindowViewModel> _logger;
    public MainWindowViewModel(ILogger<MainWindowViewModel> logger)
    {
        _logger = logger;
        LocalizedStrings.Instance.PropertyChanged += (s, e) =>
            OnPropertyChanged(string.Empty);
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
        current = CultureInfo.CurrentUICulture;
        _logger.LogInformation("CultureInfo changed to {current}", current);
    }
    [RelayCommand]
    void ThrowException()
    {
        throw new InvalidOperationException("Test unhandled exception!");
    }
}
