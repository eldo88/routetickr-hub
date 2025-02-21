using RouteTickrAPI.DTOs;
using RouteTickrAPI.Mappers;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Tests.TestHelpers;

namespace RouteTickrAPI.Tests.MapperTests;

[TestFixture]
public class TickMapperTests
{
    [Test]
    public void ToTickDto_MapsTickToTickDto_Correctly()
    {
        //Arrange
        var tick = TickBuilder.CreateValidTick();
        //Act
        var result = TickMapper.ToTickDto(tick);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tick.Id));
            Assert.That(result.Date, Is.EqualTo(tick.Date));
            Assert.That(result.Route, Is.EqualTo(tick.Route));
            Assert.That(result.Rating, Is.EqualTo(tick.Rating));
            Assert.That(result.Notes, Is.EqualTo(tick.Notes));
            Assert.That(result.Url, Is.EqualTo(tick.Url));
            Assert.That(result.Pitches, Is.EqualTo(tick.Pitches));
            Assert.That(result.Location, Is.EqualTo(tick.Location));
            Assert.That(result.AvgStars, Is.EqualTo(tick.AvgStars));
            Assert.That(result.YourStars, Is.EqualTo(tick.YourStars));
            Assert.That(result.Style, Is.EqualTo(tick.Style));
            Assert.That(result.LeadStyle, Is.EqualTo(tick.LeadStyle));
            Assert.That(result.RouteType, Is.EqualTo(tick.RouteType));
            Assert.That(result.YourRating, Is.EqualTo(tick.YourRating));
            Assert.That(result.Length, Is.EqualTo(tick.Length));
            Assert.That(result.RatingCode, Is.EqualTo(tick.RatingCode));
        });
    }
    
    [Test]
    public void ToTickDto_ReturnsValidTickDto_WhenTickHasDefaultValues()
    {
        // Arrange
        var tick = new Tick();
        // Act
        var result = TickMapper.ToTickDto(tick);
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(0));
            Assert.That(result.Date, Is.EqualTo(default(DateTime)));
            Assert.That(result.Route, Is.Null);
            Assert.That(result.Rating, Is.Null);  
            Assert.That(result.Notes, Is.Null);  
            Assert.That(result.Url, Is.Null);  
            Assert.That(result.Pitches, Is.Null);
            Assert.That(result.Location, Is.Null);  
            Assert.That(result.AvgStars, Is.Null);
            Assert.That(result.YourStars, Is.Null);  
            Assert.That(result.Style, Is.Null);  
            Assert.That(result.LeadStyle, Is.Null);  
            Assert.That(result.RouteType, Is.Null);  
            Assert.That(result.YourRating, Is.Null);  
            Assert.That(result.Length, Is.Null);
            Assert.That(result.RatingCode, Is.Null);
        });
    }
    
    [Test]
    public void ToTickDto_ThrowsArgumentNullException_WhenTickIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => TickMapper.ToTickDto(null));
    }

    [Test]
    public void ToTick_MapsTickDtoToTick_Correctly()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();
        //Act
        var result = TickMapper.ToTick(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(tickDto.Id));
            Assert.That(result.Date, Is.EqualTo(tickDto.Date));
            Assert.That(result.Route, Is.EqualTo(tickDto.Route));
            Assert.That(result.Rating, Is.EqualTo(tickDto.Rating));
            Assert.That(result.Notes, Is.EqualTo(tickDto.Notes));
            Assert.That(result.Url, Is.EqualTo(tickDto.Url));
            Assert.That(result.Pitches, Is.EqualTo(tickDto.Pitches));
            Assert.That(result.Location, Is.EqualTo(tickDto.Location));
            Assert.That(result.AvgStars, Is.EqualTo(tickDto.AvgStars));
            Assert.That(result.YourStars, Is.EqualTo(tickDto.YourStars));
            Assert.That(result.Style, Is.EqualTo(tickDto.Style));
            Assert.That(result.LeadStyle, Is.EqualTo(tickDto.LeadStyle));
            Assert.That(result.RouteType, Is.EqualTo(tickDto.RouteType));
            Assert.That(result.YourRating, Is.EqualTo(tickDto.YourRating));
            Assert.That(result.Length, Is.EqualTo(tickDto.Length));
            Assert.That(result.RatingCode, Is.EqualTo(tickDto.RatingCode));
        });
    }
    
    [Test]
    public void ToTick_ReturnsValidTick_WhenTickDtoHasDefaultValues()
    {
        // Arrange
        var tickDto = new TickDto();
        // Act
        var result = TickMapper.ToTick(tickDto);
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(0)); 
            Assert.That(result.Date, Is.EqualTo(default(DateTime)));
            Assert.That(result.Route, Is.EqualTo(string.Empty));  
            Assert.That(result.Rating, Is.EqualTo(string.Empty));  
            Assert.That(result.Notes, Is.EqualTo(string.Empty));  
            Assert.That(result.Url, Is.EqualTo(string.Empty));  
            Assert.That(result.Pitches, Is.Null);
            Assert.That(result.Location, Is.EqualTo(string.Empty));  
            Assert.That(result.AvgStars, Is.Null);
            Assert.That(result.YourStars, Is.Null);  
            Assert.That(result.Style, Is.EqualTo(string.Empty));  
            Assert.That(result.LeadStyle, Is.EqualTo(string.Empty));  
            Assert.That(result.RouteType, Is.EqualTo(string.Empty));  
            Assert.That(result.YourRating, Is.EqualTo(string.Empty));  
            Assert.That(result.Length, Is.Null);
            Assert.That(result.RatingCode, Is.Null);
        });
    }
    
    [Test]
    public void ToTick_ThrowsArgumentNullException_WhenTickDtoIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => TickMapper.ToTick(null));
    }

}