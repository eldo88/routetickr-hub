using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Tests.TestHelpers;

public class TickDtoComparer : IEqualityComparer<TickDto>
{
    public bool Equals(TickDto? x, TickDto? y)
    {
        if (x is null || y is null)
        {
            return false;
        }
        
        return x.Id == y.Id
               && x.Date == y.Date
               && x.Route == y.Route
               && x.Rating == y.Rating
               && x.Notes == y.Notes
               && x.Url == y.Url
               && x.Pitches == y.Pitches
               && x.Location == y.Location
               && x.AvgStars == y.AvgStars
               && x.YourStars == y.YourStars
               && x.Style == y.Style
               && x.LeadStyle == y.LeadStyle
               && x.RouteType == y.RouteType
               && x.YourRating == y.YourRating
               && x.Length == y.Length
               && x.RatingCode == y.RatingCode;
    }

    public int GetHashCode(TickDto obj)
    {
        return obj.Id.GetHashCode();
    }
}