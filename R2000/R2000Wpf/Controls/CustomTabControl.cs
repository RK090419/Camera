using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.Input;

namespace R2000Wpf.Controls;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:R2000Wpf.Controls"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:R2000Wpf.Controls;assembly=R2000Wpf.Controls"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:CustomTabControl/>
///
/// </summary>
public class CustomTabControl : TabControl
{
    public CustomTabControl()
    {
        this.Loaded += CustomTabControl_Loaded;
        this.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
    }

    private void CustomTabControl_Loaded(object sender, RoutedEventArgs e)
    {
        AttachAdornersToItems();
    }

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomTabControl));
    private void ItemContainerGenerator_StatusChanged(object? sender, EventArgs e)
    {
        if (ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
        {
            AttachAdornersToItems();
        }
    }
    private void AttachAdornersToItems()
    {
        foreach (var item in Items)
        {
            if (ItemContainerGenerator.ContainerFromItem(item) is TabItem tabItem)
            {
                var layer = AdornerLayer.GetAdornerLayer(tabItem);
                if (layer == null || layer.GetAdorners(tabItem)?.Any(a => a is RAdorner) == true)
                    continue;

                var btn = new Button
                {
                    Style = (Style)FindResource("TubItemCloseBtn"),
                    Command = new RelayCommand(() => this.Items.Remove(item))
                };

                var adorner = new RAdorner(tabItem, btn, new Thickness(2, 0, 0, 0));
                layer?.Add(adorner);
            }
        }
    }


}
