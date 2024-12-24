namespace RouteTickrAPI.Models;

public class Tick
{
    public Tick() {}
    
    public Tick(DateTime date, string route, string rating, string notes, string url, int? pitches, string location, double? averageStars, double? userStars, string style, string leadStyle, string routeType, string userRating, double? length, int? ratingCode)
    {
        Date = date;
        Route = route;
        Rating = rating;
        Notes = notes;
        Url = url;
        Pitches = pitches;
        Location = location;
        AverageStars = averageStars;
        UserStars = userStars;
        Style = style;
        LeadStyle = leadStyle;
        RouteType = routeType;
        UserRating = userRating;
        Length = length;
        RatingCode = ratingCode;
    }
//TODO add id when database is hooked up
    public DateTime Date { get; set; }
    public string Route { get; set; }
    public string Rating { get; set; }
    public string Notes { get; set; }
    public string Url { get; set; }
    public int? Pitches { get; set; }
    public string Location { get; set; }
    public double? AverageStars { get; set; }
    public double? UserStars { get; set; }
    public string Style { get; set; }
    public string LeadStyle { get; set; }
    public string RouteType { get; set; }
    public string UserRating { get; set; }
    public double? Length { get; set; }
    public int? RatingCode { get; set; }
}