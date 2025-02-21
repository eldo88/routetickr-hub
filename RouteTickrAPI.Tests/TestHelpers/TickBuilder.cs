using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

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
            YourRating = "5.10d"
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
            YourRating = "5.12a"
        };
    }
}