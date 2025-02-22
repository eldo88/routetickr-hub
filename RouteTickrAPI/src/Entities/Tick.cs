using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteTickrAPI.Entities;

public class Tick
{
    public Tick() {}
    
    public Tick(DateTime date, string route, string rating, string notes, string url, int? pitches, string location, double? avgStars, double? yourStars, string style, string leadStyle, string routeType, string yourRating, double? length, int? ratingCode)
    {
        //Id = id;
        Date = date;
        Route = route;
        Rating = rating;
        Notes = notes;
        Url = url;
        Pitches = pitches;
        Location = location;
        AvgStars = avgStars;
        YourStars = yourStars;
        Style = style;
        LeadStyle = leadStyle;
        RouteType = routeType;
        YourRating = yourRating;
        Length = length;
        RatingCode = ratingCode;
    }

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
}