using RouteTickrAPI.Builders;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Extensions;

public static class TickDtoExtensions
{
    public static TickDto ToTickDto(this Tick entity)
    {
        ArgumentNullException.ThrowIfNull(entity, "Tick is null in ToTickDto");

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
        ArgumentNullException.ThrowIfNull(dto, "Dto is null in ToTickEntity");
        ArgumentNullException.ThrowIfNull(climb, "Climb is null in ToTickEntity");
        
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

    public static Climb BuildClimb(this TickDto tickDto)
    {
        var routeType = tickDto.RouteType.Split(',', StringSplitOptions.TrimEntries);
        if (!Enum.TryParse(routeType[0], out ClimbType climbType))
        {
            climbType = ClimbType.Unknown;
        }

        return climbType switch
        {
            ClimbType.Sport => new SportRouteBuilder()
                .WithId(tickDto.Id)
                .WithName(tickDto.Route)
                .WithRating(tickDto.Rating)
                .WithLocation(tickDto.Location)
                .WithUrl(tickDto.Url)
                .WithHeight(tickDto.Length)
                .WithDangerRating("")
                .WithClimbType(routeType[0])
                .WithNumberOfBolts(0)
                .WithNumberOfPitches(tickDto.Pitches ?? 0)
                .Build(),
            
            ClimbType.Trad => new TradRouteBuilder()
                .WithId(tickDto.Id)
                .WithName(tickDto.Route)
                .WithRating(tickDto.Rating)
                .WithLocation(tickDto.Location)
                .WithUrl(tickDto.Url)
                .WithHeight(tickDto.Length)
                .WithDangerRating("")
                .WithClimbType(routeType[0])
                .WithNumberOfPitches(tickDto.Pitches ?? 0)
                .WithGearNeeded("")
                .Build(),
            
            ClimbType.Boulder => new BoulderBuilder()
                .WithId(tickDto.Id)
                .WithName(tickDto.Route)
                .WithRating(tickDto.Rating)
                .WithLocation(tickDto.Location)
                .WithUrl(tickDto.Url)
                .WithHeight(tickDto.Length)
                .WithDangerRating("")
                .WithClimbType(routeType[0])
                .WithHasTopOut(true)
                .WithNumberOfCrashPadsNeeded(2)
                .WithIsTraverse(false)
                .Build(),
            
            ClimbType.Alpine => new AlpineRockRouteBuilder()
                .WithId(tickDto.Id)
                .WithName(tickDto.Route)
                .WithRating(tickDto.Rating)
                .WithLocation(tickDto.Location)
                .WithUrl(tickDto.Url)
                .WithHeight(tickDto.Length)
                .WithDangerRating("")
                .WithClimbType(routeType[0])
                .WithNumberOfPitches(tickDto.Pitches ?? 0)
                .WithApproachDistance(0)
                .WithGearNeeded("")
                .Build(),
            
            ClimbType.Ice => new IceClimbBuilder()
                .WithId(tickDto.Id)
                .WithName(tickDto.Route)
                .WithRating(tickDto.Rating)
                .WithLocation(tickDto.Location)
                .WithUrl(tickDto.Url)
                .WithHeight(tickDto.Length)
                .WithDangerRating("")
                .WithClimbType(routeType[0])
                .WithIceType("Ice")
                .WithNumberOfPitches(tickDto.Pitches ?? 0)
                .Build(),
            
            _ => throw new ArgumentException("Error creating routes")
        };
    }

}