using RouteTickr.Messages.Enums;

namespace RouteTickr.Entities;

public class TradRoute : Climb
{
    public int NumberOfPitches { get; init; }
    public string GearNeeded { get; init; } = string.Empty;

    public TradRoute() => ClimbType = ClimbType.Trad;
}