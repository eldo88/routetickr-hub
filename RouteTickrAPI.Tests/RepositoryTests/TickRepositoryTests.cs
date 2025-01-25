using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Tests.TestHelpers;

namespace RouteTickrAPI.Tests.RepositoryTests;

[TestFixture]
public class TickRepositoryTests
{
    private ApplicationDbContext _context;
    private TickRepository _tickRepository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _tickRepository = new TickRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsList_WhenTicksExist()
    {
        //Arrange
        var ticks = new List<Tick>()
        {
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick()
        };
        await _context.AddRangeAsync(ticks);
        await _context.SaveChangesAsync();
        //Act
        var result = await _tickRepository.GetAllAsync();
        //Assert
        Assert.That(result, Is.TypeOf(typeof(List<Tick>)));
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyCollection_WhenNoTicksExist()
    {
        //Arrange
        _context.RemoveRange(_context.Ticks);
        await _context.SaveChangesAsync();
        //Act
        var result = await _tickRepository.GetAllAsync();
        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllTicks_WhenTicksExist()
    {
        //Arrange
        var ticks = new List<Tick>()
        {
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick()
        };
        await _context.AddRangeAsync(ticks);
        await _context.SaveChangesAsync();
        //Act
        var result = await _tickRepository.GetAllAsync();
        //Assert
        Assert.That(result.Count(), Is.EqualTo(ticks.Count));
        CollectionAssert.AreEquivalent(ticks, result);
    }

    [Test]
    public async Task GetAllAsync_DoesNotModifyContext()
    {
        //Arrange
        var ticks = new List<Tick>()
        {
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick()
        };
        await _context.AddRangeAsync(ticks);
        await _context.SaveChangesAsync();
        //Act
        var result = await _tickRepository.GetAllAsync();
        //Assert
        Assert.That(_context.Ticks.Local.Count, Is.EqualTo(2));
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync_CanHandleLargeNumberOfTicks()
    {
        //Arrange
        const int recordCount = 5000;
        
        var ticks = Enumerable.Range(1, recordCount)
            .Select(_ => TickBuilder.CreateValidTick())
            .ToList();
        
        await _context.AddRangeAsync(ticks);
        await _context.SaveChangesAsync();
        //Act
        var result = await _tickRepository.GetAllAsync();
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(recordCount));
        CollectionAssert.AreEquivalent(ticks, result);
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsTick_WhenTickExists()
    {
        // Arrange
        var tick = TickBuilder.CreateValidTick();
        await _context.Ticks.AddAsync(tick);
        await _context.SaveChangesAsync();
        // Act
        var result = await _tickRepository.GetByIdAsync(tick.Id);
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(tick.Id));
        Assert.That(result.Date, Is.EqualTo(tick.Date));
        Assert.That(result.Length, Is.EqualTo(tick.Length));
        Assert.That(result.Pitches, Is.EqualTo(tick.Pitches));
        Assert.That(result.Location, Is.EqualTo(tick.Location));
        Assert.That(result.Notes, Is.EqualTo(tick.Notes));
        Assert.That(result.Rating, Is.EqualTo(tick.Rating));
        Assert.That(result.Route, Is.EqualTo(tick.Route));
        Assert.That(result.Style, Is.EqualTo(tick.Style));
        Assert.That(result.Url, Is.EqualTo(tick.Url));
        Assert.That(result.AvgStars, Is.EqualTo(tick.AvgStars));
        Assert.That(result.LeadStyle, Is.EqualTo(tick.LeadStyle));
        Assert.That(result.RatingCode, Is.EqualTo(tick.RatingCode));
        Assert.That(result.RouteType, Is.EqualTo(tick.RouteType));
        Assert.That(result.YourRating, Is.EqualTo(tick.YourRating));
        Assert.That(result.YourStars, Is.EqualTo(tick.YourStars));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenTickDoesNotExist()
    {
        // Act
        var result = await _tickRepository.GetByIdAsync(9999);
        // Assert
        Assert.That(result, Is.Null);
    }


    [Test]
    public async Task DeleteAsync_DeletesTick_WhenTickExists()
    {
        // Arrange
        var tick = TickBuilder.CreateValidTick();
        _context.Ticks.Add(tick);
        await _context.SaveChangesAsync();
        // Act
        var result = await _tickRepository.DeleteAsync(tick.Id);
        // Assert
        Assert.That(result, Is.True);
        var deletedTick = await _context.Ticks.FindAsync(tick.Id);
        Assert.That(deletedTick, Is.Null);
    }
    
    [Test]
    public async Task AddAsync_AddsTickToDatabase_WhenValidTickIsProvided()
    {
        // Arrange
        var tick = TickBuilder.CreateValidTick();
        // Act
        var result = await _tickRepository.AddAsync(tick);
        // Assert
        Assert.That(result, Is.True);
        var addedTick = await _context.Ticks.FindAsync(tick.Id);
        Assert.That(addedTick, Is.Not.Null);
        Assert.That(addedTick.Id, Is.EqualTo(tick.Id));
        Assert.That(addedTick.Date, Is.EqualTo(tick.Date));
        Assert.That(addedTick.Length, Is.EqualTo(tick.Length));
        Assert.That(addedTick.Pitches, Is.EqualTo(tick.Pitches));
        Assert.That(addedTick.Location, Is.EqualTo(tick.Location));
        Assert.That(addedTick.Notes, Is.EqualTo(tick.Notes));
        Assert.That(addedTick.Rating, Is.EqualTo(tick.Rating));
        Assert.That(addedTick.Route, Is.EqualTo(tick.Route));
        Assert.That(addedTick.Style, Is.EqualTo(tick.Style));
        Assert.That(addedTick.Url, Is.EqualTo(tick.Url));
        Assert.That(addedTick.AvgStars, Is.EqualTo(tick.AvgStars));
        Assert.That(addedTick.LeadStyle, Is.EqualTo(tick.LeadStyle));
        Assert.That(addedTick.RatingCode, Is.EqualTo(tick.RatingCode));
        Assert.That(addedTick.RouteType, Is.EqualTo(tick.RouteType));
        Assert.That(addedTick.YourRating, Is.EqualTo(tick.YourRating));
        Assert.That(addedTick.YourStars, Is.EqualTo(tick.YourStars));
    }
    
    [Test]
    public async Task UpdateAsync_UpdatesExistingTick_WhenTickExists()
    {
        // Arrange
        var tick = TickBuilder.CreateValidTick();
        await _context.Ticks.AddAsync(tick);
        await _context.SaveChangesAsync();
        tick.Route = "Updated Route name";
        // Act
        var result = await _tickRepository.UpdateAsync(tick);
        // Assert
        Assert.That(result, Is.True);
        var updatedTick = await _context.Ticks.FindAsync(tick.Id);
        Assert.That(updatedTick.Route, Is.EqualTo("Updated Route name"));
    }

    [Test]
    public async Task UpdateAsync_ReturnsFalse_WhenTickDoesNotExist()
    {
        // Arrange
        var tick = TickBuilder.CreateValidTick();
        tick.Id = 9999;
        // Act
        var result = await _tickRepository.UpdateAsync(tick);
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_ReturnsFalse_WhenTickDoesNotExist()
    {
        // Arrange
        const int nonExistentId = 9999;
        // Act
        var result = await _tickRepository.DeleteAsync(nonExistentId);
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var ticks = new List<Tick>
        {
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick()
        };
        await _context.Ticks.AddRangeAsync(ticks);
        await _context.SaveChangesAsync();
        // Act
        var result = await _tickRepository.GetTotalCountAsync();
        // Assert
        Assert.That(result, Is.EqualTo(ticks.Count));
    }

}