using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace RemnantOverseer.Utilities;
public class GlyphNumberToIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(int))
        {
            return AvaloniaProperty.UnsetValue;
            //throw new ArgumentException("Unsupported");
        }
        else
        {
            var resName = $"Glyph_{value}";
            _ = Application.Current!.TryGetResource(resName, out var result);
            return result ?? throw new ArgumentException("Resource is 'null'"); ;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
