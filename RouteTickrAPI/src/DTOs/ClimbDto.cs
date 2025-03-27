using RouteTickr.Messages.Enums;

namespace RouteTickrAPI.DTOs;

public class ClimbDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public double? Height { get; set; }
    public ClimbDangerRating? DangerRating { get; set; }
    public ClimbType ClimbType { get; set; }
    
    public int? NumberOfBolts { get; init; }
    public int? NumberOfPitches { get; init; }
    
    public string GearNeeded { get; init; } = string.Empty;
    public decimal? ApproachDistance { get; init; }
    
    public bool? HasTopOut { get; init; }
    public int? NumberOfCrashPadsNeeded { get; init; }
    public bool? IsTraverse { get; init; }
    
    public string IceType { get; init; } = string.Empty;
}