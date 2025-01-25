namespace RouteTickrAPI.DTOs;

public class ClimbingStatsDto
{
    public int TotalTicks { get; set; }
    public int? TotalPitches { get; set; }
    public Dictionary<string, int> LocationVisits { get; set; } = new();
}