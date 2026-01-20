using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace R2000Wpf.Converters;

public class RectFromBorder : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double width = (double)values[0];
        double height = (double)values[1];
        double radius = 0;
        if (values[2] is double d)
            radius = d;
        else if (values[2] is CornerRadius cr)
            radius = cr.TopLeft;

        return new RectangleGeometry
        {
            Rect = new Rect(0, 0, width, height),
            RadiusX = radius,
            RadiusY = radius
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
