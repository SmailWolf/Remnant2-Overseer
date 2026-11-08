using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace RemnantOverseer.Utilities;
public class DoubleToCornerRadiusConverter: IValueConverter
{
    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not double cornerRadius)
        {
            return AvaloniaProperty.UnsetValue;
        }

        return new CornerRadius(cornerRadius);
    }

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
