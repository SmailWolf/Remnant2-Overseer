using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using RemnantOverseer.Models.Enums;
using System;
using System.Globalization;

namespace RemnantOverseer.Utilities;
public class ItemTypeToIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(ItemTypes))
        {
            throw new ArgumentException("Unsupported");
        }
        else
        {
            var resName = $"{value}Icon";
            _ = Application.Current!.TryGetResource(resName, out var result);
            return result ?? throw new ArgumentException("Resource is 'null'"); ;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
