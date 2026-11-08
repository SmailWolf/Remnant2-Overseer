using RemnantOverseer.Models.Enums;
using System.Collections.Generic;

namespace RemnantOverseer.Utilities;
public static class ArchetypeColors
{
    public static Dictionary<Archetypes, string> Map { get; } = new()
    {
        { Archetypes.Alchemist, "#12271f" },
        { Archetypes.Archon, "#152026" },
        { Archetypes.Challenger, "#1f1c17" },
        { Archetypes.Engineer, "#171723" },
        { Archetypes.Explorer, "#132016" },
        { Archetypes.Gunslinger, "#271515" },
        { Archetypes.Handler, "#242215" },
        { Archetypes.Hunter, "#291713" },
        { Archetypes.Invader, "#241726" },
        { Archetypes.Invoker, "#181c1f" },
        { Archetypes.Medic, "#11261d" },
        { Archetypes.Ritualist, "#231228" },
        { Archetypes.Summoner, "#1e1613" },
        { Archetypes.Warden, "#34393c" },
        { Archetypes.Unknown, "#301e1e" },
    };
}
