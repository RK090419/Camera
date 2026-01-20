using System.ComponentModel;
using System.Windows;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using R2000Wpf.ViewModels;

namespace R2000Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    INavigationService Nav => DesignerProperties.GetIsInDesignMode(new DependencyObject()) ? null! : App.Current.Services!.GetRequiredService<INavigationService>();

    public MainWindow(MainWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();

    }
    private void PageFrame_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return;
        }
        Nav.Navigate<LoginView>();
    }
}