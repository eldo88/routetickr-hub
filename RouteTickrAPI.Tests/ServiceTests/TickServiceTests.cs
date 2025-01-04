using Moq;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;

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