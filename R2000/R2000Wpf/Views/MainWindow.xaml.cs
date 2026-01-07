using System.Windows;
using R2000Wpf.ViewModels;

namespace R2000Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }
}