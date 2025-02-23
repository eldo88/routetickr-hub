using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

public class TradRoute : Climb
{
    public int NumberOfPitches { get; init; }
    public string GearNeeded { get; init; } = string.Empty;

    public TradRoute() => ClimbType = ClimbType.Trad;
}