using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Extensions;

public static class TickDtoExtensions
{
    public static TickDto ToTickDto(this Tick entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TickDto
        {
            Id = entity.Id,
            Date = entity.Date,
            Route = entity.Route,
            Rating = entity.Rating,
            Notes = entity.Notes,
            Url = entity.Url,
            Pitches = entity.Pitches,
            Location = entity.Location,
            AvgStars = entity.AvgStars,
            YourStars = entity.YourStars,
            Style = entity.Style,
            LeadStyle = entity.LeadStyle,
            RouteType = entity.RouteType,
            YourRating = entity.YourRating,
            Length = entity.Length,
            RatingCode = entity.RatingCode,
            ClimbId = entity.ClimbId,
            Climb = entity.Climb
        };
    }
    
    public static Tick ToEntity(this TickDto dto)
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
            RatingCode = dto.RatingCode,
            ClimbId = dto.ClimbId ?? 0,
            Climb = dto.Climb ?? new Boulder() //TODO need to create a builder for derived types
        };
    }
    
    public static Tick ToTickEntity(this TickDto dto, Climb climb)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(climb);
        
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
            RatingCode = dto.RatingCode,
            ClimbId = climb.Id,
            Climb = climb
        };
    }

}