using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Enums;

namespace RouteTickrAPI.Tests.TestHelpers;

public static class TickBuilder
{
    public static Tick CreateValidTick(int id = 0)
    {
        return new Tick
        {
            Id = id,
            LeadStyle = "Sport",
            Location = "Red River Gorge",
            Notes = "Heady",
            Rating = "5.10d",
            Route = "Breakfast Burrito",
            RouteType = "Sport",
            Style = "On-sight",
            Url = "https://example.com/tick",
            Pitches = 1,
            YourRating = "5.10d",
            Length = 50,
            ClimbId = 0,
            Climb = new SportRoute()
            {
                ClimbType = ClimbType.Sport,
                DangerRating = ClimbDangerRating.G,
                Height = 50,
                Id = 0,
                Location = "Red River Gorge",
                Name = "Breakfast Burrito",
                NumberOfBolts = 7,
                NumberOfPitches = 1,
                Rating = "5.10d",
                Url = "https://example.com/tick"
            }
        };
    }

    public static TickDto CreateValidTickDto(int id = 0)
    {
        return new TickDto
        {
            Id = id,
            LeadStyle = "Sport",
            Location = "Rifle",
            Notes = "Heady",
            Rating = "5.12a",
            Route = "Baby Brother",
            RouteType = "Sport",
            Style = "On-sight",
            Url = "https://example.com/tick",
            YourRating = "5.12a",
            Length = 50,
            ClimbId = 0,
            Climb = new SportRoute()
            {
                ClimbType = ClimbType.Sport,
                DangerRating = ClimbDangerRating.G,
                Height = 50,
                Id = 0,
                Location = "Rifle",
                Name = "Baby Brother",
                NumberOfBolts = 7,
                NumberOfPitches = 1,
                Rating = "5.10d",
                Url = "https://example.com/tick"
            }
        };
    }
}