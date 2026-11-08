using Avalonia.Media.Fonts;
using System;

namespace RemnantOverseer.Utilities;
internal class MontserratFontCollection: EmbeddedFontCollection
{
    public MontserratFontCollection() : base(
        new Uri("fonts:Montserrat", UriKind.Absolute),
        new Uri("avares://RemnantOverseer/Assets/Fonts", UriKind.Absolute))
    {
    }
}
