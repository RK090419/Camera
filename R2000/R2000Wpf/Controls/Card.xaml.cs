using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace R2000Wpf.Controls;

[ContentProperty(nameof(CardContent))]
public partial class Card : UserControl
{
    public Card()
    {
        InitializeComponent();
        ContentBorder.SizeChanged += OnSizeChange;
    }
    void OnSizeChange(object? sender, SizeChangedEventArgs e)
    {
        if (sender is Border b)
            b.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, b.ActualWidth, b.ActualHeight),
                RadiusX = b.CornerRadius.TopLeft,
                RadiusY = b.CornerRadius.TopLeft
            };

    }

    public static readonly DependencyProperty CardContentProperty = DependencyProperty.Register(
    "CardContent", typeof(UIElement),
    typeof(Card)
    );

    public UIElement CardContent
    {
        get => (UIElement)GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }



}
