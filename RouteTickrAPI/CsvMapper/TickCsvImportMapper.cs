using CsvHelper.Configuration;
using RouteTickrAPI.Models;

namespace RouteTickrAPI.CsvMapper;

public class TickCsvImportMapper : ClassMap<Tick>
{
    public TickCsvImportMapper()
    {
        Map(t => t.Date).Name("Date");
        Map(t => t.Route).Name("Route");
        Map(t => t.Rating).Name("Rating");
        Map(t => t.Notes).Name("Notes");
        Map(t => t.Url).Name("URL");
        Map(t => t.Pitches).Name("Pitches");
        Map(t => t.Location).Name("Location");
        Map(t => t.AvgStars).Name("Avg Stars");
        Map(t => t.YourStars).Name("Your Stars");
        Map(t => t.Style).Name("Style");
        Map(t => t.LeadStyle).Name("Lead Style");
        Map(t => t.RouteType).Name("Route Type");
        Map(t => t.YourRating).Name("Your Rating");
        Map(t => t.Length).Name("Length");
        Map(t => t.RatingCode).Name("Rating Code");
    }
}
