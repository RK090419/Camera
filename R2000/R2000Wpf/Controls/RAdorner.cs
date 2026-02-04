using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace R2000Wpf.Controls;

public sealed class RAdorner : Adorner
{
    private readonly UIElement _content;
    private readonly VisualCollection _visuals;
    private readonly Thickness _offset;
    public RAdorner(UIElement adornedElement, UIElement content, Thickness offset = new Thickness()) : base(adornedElement)
    {
        _offset = offset;
        _content = content ?? throw new ArgumentNullException(nameof(content));
        _visuals = new VisualCollection(this) { _content };
        IsHitTestVisible = true;
    }
    protected override Size MeasureOverride(Size constraint)
    {
        _content.Measure(constraint);
        return _content.DesiredSize;
    }
    protected override Size ArrangeOverride(Size finalSize)
    {
        _content.Arrange(new Rect(
        _offset.Left,
        _offset.Top,
        _content.DesiredSize.Width,
        _content.DesiredSize.Height));

        return finalSize;
    }

    protected override int VisualChildrenCount => _visuals.Count;
    protected override Visual GetVisualChild(int index) => _visuals[index];
}