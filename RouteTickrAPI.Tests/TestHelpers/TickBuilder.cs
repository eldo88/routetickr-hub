using RouteTickrAPI.Models;

namespace RouteTickrAPI.Tests.TestHelpers;

public class TickBuilder
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
}