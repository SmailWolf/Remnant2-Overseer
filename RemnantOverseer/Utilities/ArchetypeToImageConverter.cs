using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using RemnantOverseer.Models.Enums;
using System;
using System.Globalization;

namespace RemnantOverseer.Utilities;
public class ArchetypeToImageConverter: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(Archetypes))
        {
            return Avalonia.AvaloniaProperty.UnsetValue;
        }
        else
        {
            string name = (Archetypes)value switch
            {
                Archetypes.Alchemist => "Alchemist",
                Archetypes.Archon => "Arcanist",
                Archetypes.Challenger => "Defender",
                Archetypes.Engineer => "Engineer",
                Archetypes.Explorer => "Explorer",
                Archetypes.Gunslinger => "Gunslinger",
                Archetypes.Handler => "Handler",
                Archetypes.Hunter => "Sharpshooter",
                Archetypes.Invader => "Invader",
                Archetypes.Invoker => "Invoker",
                Archetypes.Medic => "Medic",
                Archetypes.Ritualist => "Ritualist",
                Archetypes.Summoner => "Summoner",
                Archetypes.Warden => "Warden",
                Archetypes.Unknown => "Unknown",
                _ => throw new NotImplementedException(),
            };

            return new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Archetypes/T_UI_Icon_Archetype_{name}.png")));
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
