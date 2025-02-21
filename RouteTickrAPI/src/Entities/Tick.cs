using System.ComponentModel.DataAnnotations;

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
    public DateTime Date { get; set; }
    public string Route { get; set; }
    public string Rating { get; set; }
    public string Notes { get; set; }
    public string Url { get; set; }
    public int? Pitches { get; set; }
    public string Location { get; set; }
    public double? AvgStars { get; set; }
    public double? YourStars { get; set; }
    public string Style { get; set; }
    public string LeadStyle { get; set; }
    public string RouteType { get; set; }
    public string YourRating { get; set; }
    public double? Length { get; set; }
    public int? RatingCode { get; set; }
}