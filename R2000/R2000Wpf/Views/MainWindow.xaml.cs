using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        MainBorder.SizeChanged += OnSizeChanged;
    }
    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, border.ActualWidth, border.ActualHeight),
                RadiusX = border.CornerRadius.TopLeft,
                RadiusY = border.CornerRadius.TopLeft
            };
        }
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