using System.ComponentModel.DataAnnotations;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Entities;

public abstract class Climb
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public ClimbDangerRating DangerRating { get; set; }
    public ClimbType Type { get; set; }
}