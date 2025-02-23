using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

public class AlpineRockRoute : Climb
{
    public int NumberOfPitches { get; init; }
    public string GearNeeded { get; init; } = string.Empty;
    public decimal ApproachDistance { get; init; }

    public AlpineRockRoute() => ClimbType = ClimbType.Alpine;
}