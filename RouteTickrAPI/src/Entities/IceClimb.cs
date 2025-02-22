namespace RouteTickrAPI.Entities;

public class IceClimb : Climb
{
    public string IceType { get; set; } = string.Empty;
    public int NumberOfPitches { get; set; }
}