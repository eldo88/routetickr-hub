namespace RouteTickrAPI.Entities;

public class TradRoute : Climb
{
    public int NumberOfPitches { get; set; }
    public string GearNeeded { get; set; } = string.Empty;
}