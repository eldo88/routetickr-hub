using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
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
    public async Task DeleteAsync_ReturnsFalse_WhenTickDoesNotExist()
    {
        // Arrange
        const int nonExistentId = 9999;
        // Act
        var result = await _tickRepository.DeleteAsync(nonExistentId);
        // Assert
        Assert.That(result, Is.False);
    }
}