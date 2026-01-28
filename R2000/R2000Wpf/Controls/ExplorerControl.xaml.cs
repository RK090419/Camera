using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Models;

namespace R2000Wpf.Controls;

public partial class ExplorerControl : UserControl
{
    public ExplorerControl()
    {
        InitializeComponent();
        // For demo purposes, add a few files
        Files.Add(new FileItem { Name = "Document1.txt", Type = FileType.Txt });
        Files.Add(new FileItem { Name = "Document2.txt", Type = FileType.Txt });

        FileList.ItemsSource = Files;
        OpenCommand = new RelayCommand(() => SetVisibility(true));
        CloseCommand = new RelayCommand(() => SetVisibility(false));
    }
    public ICommand CloseCommand { get; }
    public ICommand OpenCommand { get; }
    public bool IsOpen => this.Visibility == Visibility.Visible;
    public event EventHandler? IsOpenChanged;

    public ObservableCollection<FileItem> Files { get; set; } = new();
    private void SetVisibility(bool visible)
    {
        this.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        IsOpenChanged?.Invoke(this, EventArgs.Empty);
    }
    private void Item_LeftClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is FileItem file)
        {
            // TODO: open file in main tab control
            MessageBox.Show($"Left-click: {file.Name}");
        }
    }

    private void Item_RightClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is FileItem file)
        {
            // TODO: show context menu with Add/Delete/Edit
            MessageBox.Show($"Right-click: {file.Name}");
        }
    }
}
