using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Mappers;

public static class TickMapper
{
    public static TickDto ToTickDto(Tick tick)
    {
        ArgumentNullException.ThrowIfNull(tick);

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
    
    public static Tick ToTick(TickDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        return new Tick
        {
            Id = dto.Id,
            Date = dto.Date,
            Route = dto.Route,
            Rating = dto.Rating,
            Notes = dto.Notes,
            Url = dto.Url,
            Pitches = dto.Pitches,
            Location = dto.Location,
            AvgStars = dto.AvgStars,
            YourStars = dto.YourStars,
            Style = dto.Style,
            LeadStyle = dto.LeadStyle,
            RouteType = dto.RouteType,
            YourRating = dto.YourRating,
            Length = dto.Length,
            RatingCode = dto.RatingCode
        };
    }

}