using System.Text;
using Microsoft.AspNetCore.Http;

namespace RouteTickrAPI.Tests.TestHelpers;

public static class ImportFileHelper
{
    private const string MultipleTickStr =  @"Date,Route,Rating,Notes,URL,Pitches,Location,""Avg Stars"",""Your Stars"",Style,""Lead Style"",""Route Type"",""Your Rating"",Length,""Rating Code""
        2025-02-02,""Handcracker Direct"",5.10a,""W/ Bryan. Led p2"",https://www.mountainproject.com/route/105749599/handcracker-direct,3,""Colorado > Boulder > Eldorado Canyon State Park > The West Ridge > West Ridge - part C - Pony Express to Long John"",3.6,-1,Lead,Redpoint,Trad,,,2600
        2024-12-01,""Jade Gate"",5.11+,""A little awkward and hard to read but not too bad once you know the moves."",https://www.mountainproject.com/route/105752491/jade-gate,1,""Colorado > Boulder > Flatirons > South > Seal Rock"",2.7,-1,Lead,Fell/Hung,Sport,,,5300";

    private const string SingleTickStr = @"Date,Route,Rating,Notes,URL,Pitches,Location,""Avg Stars"",""Your Stars"",Style,""Lead Style"",""Route Type"",""Your Rating"",Length,""Rating Code""
        2025-02-02,""Handcracker Direct"",5.10a,""W/ Bryan. Led p2"",https://www.mountainproject.com/route/105749599/handcracker-direct,3,""Colorado > Boulder > Eldorado Canyon State Park > The West Ridge > West Ridge - part C - Pony Express to Long John"",3.6,-1,Lead,Redpoint,Trad,,,2600";
    
    public static IFormFile CreateMockImportFileWithMultipleTicks()
    {
        var csvBytes = Encoding.UTF8.GetBytes(MultipleTickStr);
        var stream = new MemoryStream(csvBytes);

        return new FormFile(stream, 0, csvBytes.Length, "Data", "ticks.csv");
    }

    public static IFormFile CreateMockImportFileWithSingleTick()
    {
        var csvBytes = Encoding.UTF8.GetBytes(SingleTickStr);
        var stream = new MemoryStream(csvBytes);
        
        return new FormFile(stream, 0, csvBytes.Length, "Data", "ticks.csv");
    }
}