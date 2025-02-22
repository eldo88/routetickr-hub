namespace RouteTickrAPI.Entities;

public class AlpineRockRoute : Climb
{
    public int NumberOfPitches { get; set; }
    public string GearNeeded { get; set; } = string.Empty;
    public decimal ApproachDistance { get; set; }
}