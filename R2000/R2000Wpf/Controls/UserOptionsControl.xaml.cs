using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Resources;

namespace R2000Wpf.Controls;
public partial class UserOptionsControl : UserControl
{
    public UserOptionsControl()
    {
        InitializeComponent();
        LocalizedStrings.Instance.PropertyChanged += (_, __) => HandleExplorerBtn();

        ToggleExplorerCommand = new RelayCommand(
        execute: () =>
        {
            if (Explorer!.IsOpen)
            {
                Explorer.CloseCommand.Execute(null);
            }
            else
            {
                Explorer.OpenCommand.Execute(null);
            }
            HandleExplorerBtn();
        },
        canExecute: () => Explorer != null);

    }
    public ExplorerControl? Explorer { get; set; }
    public ICommand ToggleExplorerCommand { get; }

    private void HandleExplorerBtn()
    {
        if (Explorer is null)
        {
            ExplorerBtn.Visibility = Visibility.Collapsed;
            return;
        }
        ExplorerBtn.Visibility = Visibility.Visible;
        ExplorerBtn.Content = Explorer.IsOpen ?
            UserOptionsControlStrings.CloseExplorer :
            UserOptionsControlStrings.OpenExplorer;
    }
    public void SetExplorerReference(ExplorerControl explorerReference)
    {
        Explorer = explorerReference;
        Explorer.IsOpenChanged += (s, e) => HandleExplorerBtn();
        ((RelayCommand)ToggleExplorerCommand).NotifyCanExecuteChanged();
        HandleExplorerBtn();

    }
}
