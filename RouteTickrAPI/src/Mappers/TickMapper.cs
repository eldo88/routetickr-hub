using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;

namespace RouteTickrAPI.Mappers;

public static class TickMapper
{
    public static TickDto ToTickDto(Tick tick)
    {
        return new TickDto
        {
            Id = tick.Id,
            Date = tick.Date,
            Route = tick.Route,
            Rating = tick.Rating,
            Notes = tick.Notes,
            Url = tick.Url,
            Pitches = tick.Pitches,
            Location = tick.Location,
            AvgStars = tick.AvgStars,
            YourStars = tick.YourStars,
            Style = tick.Style,
            LeadStyle = tick.LeadStyle,
            RouteType = tick.RouteType,
            YourRating = tick.YourRating,
            Length = tick.Length,
            RatingCode = tick.RatingCode
        };
    }
}