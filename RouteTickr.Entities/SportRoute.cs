using RouteTickr.Messages.Enums;

namespace RouteTickr.Entities;

public class SportRoute : Climb
{
    public int NumberOfBolts { get; init; }
    public int NumberOfPitches { get; init; }

    public SportRoute() => ClimbType = ClimbType.Sport;
}