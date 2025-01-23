namespace RouteTickrAPI.DTOs;

public class TickDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Route { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int? Pitches { get; set; }
    public string Location { get; set; } = string.Empty;
    public double? AvgStars { get; set; }
    public double? YourStars { get; set; }
    public string Style { get; set; } = string.Empty;
    public string LeadStyle { get; set; } = string.Empty;
    public string RouteType { get; set; } = string.Empty;
    public string YourRating { get; set; } = string.Empty;
    public double? Length { get; set; }
    public int? RatingCode { get; set; }
}