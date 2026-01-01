using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaDemonstration.ViewModels;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaDemonstration.Views;

public partial class MainWindow : Window
{
    INavigationService Nav => Design.IsDesignMode ? null! : App.Current.Services.GetRequiredService<INavigationService>();

    public MainWindow()
    {
        InitializeComponent();
    }
    private void PageFrame_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (Design.IsDesignMode)
        {
            return;
        }
        this.DataContext = App.Current.Services.GetRequiredService<MainWindowViewModel>();
        Nav.Navigate<MainView>();
    }
}