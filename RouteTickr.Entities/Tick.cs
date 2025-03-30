using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteTickr.Entities;

public class Tick
{
    public Tick() {}
    
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; init; }
    public string Route { get; set; }
    public string Rating { get; init; }
    public string Notes { get; init; }
    public string Url { get; init; }
    public int? Pitches { get; init; }
    public string Location { get; init; }
    public double? AvgStars { get; init; }
    public double? YourStars { get; init; }
    public string Style { get; init; }
    public string LeadStyle { get; init; }
    public string RouteType { get; init; }
    public string YourRating { get; init; }
    public double? Length { get; init; }
    public int? RatingCode { get; init; }
    [Required]
    public int ClimbId { get; set; }
    [ForeignKey("ClimbId")]
    public Climb Climb { get; set; }
    [Required]
    public string UserId { get; init; }
    [ForeignKey("UserId")]
    public User User { get; init; }
}