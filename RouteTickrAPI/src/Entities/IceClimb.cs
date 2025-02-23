using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

public class IceClimb : Climb
{
    public string IceType { get; init; } = string.Empty;
    public int NumberOfPitches { get; init; }

    public IceClimb() => ClimbType = ClimbType.Ice;
}