using RouteTickrAPI.Entities;

namespace RouteTickrAPI.DTOs;

public class TickDto
{
    public int Id { get; set; }
    public DateTime Date { get; init; }
    public string Route { get; init; } = string.Empty;
    public string Rating { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int? Pitches { get; init; }
    public string Location { get; init; } = string.Empty;
    public double? AvgStars { get; init; }
    public double? YourStars { get; init; }
    public string Style { get; init; } = string.Empty;
    public string LeadStyle { get; init; } = string.Empty;
    public string RouteType { get; init; } = string.Empty;
    public string YourRating { get; init; } = string.Empty;
    public double? Length { get; init; }
    public int? RatingCode { get; init; }
    public int? ClimbId { get; set; }
    public Climb? Climb { get; init; }
}