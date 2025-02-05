using System.Collections;
using Moq;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Mappers;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;
using RouteTickrAPI.Tests.TestHelpers;

namespace RouteTickrAPI.Tests.ServiceTests;

[TestFixture]
public class TickServiceTests
{
    private Mock<ITickRepository> _tickRepository;
    private TickService _tickService;

    [SetUp]
    public void Setup()
    {
        _tickRepository = new Mock<ITickRepository>();
        _tickService = new TickService(_tickRepository.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsMappedTickDtos_WhenTicksExist()
    {
        //Arrange
        var mockTicks = new List<Tick>()
        {
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick(),
            TickBuilder.CreateValidTick()
        };
        
        _tickRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(mockTicks);
        //Act
        var result = await _tickService.GetAllAsync();
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Data.Count(), Is.EqualTo(mockTicks.Count));
            
            var expectedDtoList = mockTicks.Select(TickMapper.ToTickDto).ToList();
            Assert.That(result.Data, Is.EqualTo(expectedDtoList).Using(new TickDtoComparer()));
        });
    }

    [Test]
    public async Task GetAllAsync_ReturnsError_WhenNoTicksExist()
    {
        //Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        var mockTicks = new List<Tick>();

        _tickRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(mockTicks);
        //Act
        var result = await _tickService.GetAllAsync();
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No ticks found."));
        });
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsError_WhenRepositoryThrowsException()
    {
        // Arrange
        _tickRepository
            .Setup(r => r.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));
        // Act
        var result = await _tickService.GetAllAsync();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        });
    }
    
    [Test]
    public async Task GetAllAsync_ThrowsArgumentNullException_WhenRepositoryReturnsListWithNullTick()
    {
        // Arrange
        var mockTicks = new List<Tick> { null };

        _tickRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(mockTicks);
        // Act
        var result = await _tickService.GetAllAsync();
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("A null value was encountered while mapping ticks."));
        });
    }

    [Test]
    public async Task DeleteAsync_ReturnsSuccess_WhenDeletionIsSuccessful()
    {
        //Arrange
        const int tickId = 1;
        _tickRepository
            .Setup(r => r.DeleteAsync(tickId))
            .ReturnsAsync(true);
        //Act
        var result = await _tickService.DeleteAsync(tickId);
        //Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ReturnsError_WhenDeletionIsNotSuccessful()
    {
        //Arrange
        const int tickId = 1;
        _tickRepository
            .Setup(r => r.DeleteAsync(tickId))
            .ReturnsAsync(false);
        //Act
        var result = await _tickService.DeleteAsync(tickId);
        //Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Error deleting tick with ID: 1"));
    }
}