using System.Text.Json.Serialization;

namespace SuperMarioOdysseyOnline.Server.World;

[JsonConverter(typeof(JsonStringEnumConverter<Kingdom>))]
public enum Kingdom
{
    Cap,
    Cascade,
    Sand,
    Lake,
    Wooded,
    Cloud,
    Lost,
    Metro,
    Snow,
    Seaside,
    Luncheon,
    Ruined,
    Bowsers,
    Moon,
    Mushroom,
    DarkSide,
    DarkerSide,
    Odyssey,
}
