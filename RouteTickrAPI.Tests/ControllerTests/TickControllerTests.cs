using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RouteTickrAPI.Controllers;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Tests.ControllerTests;

[TestFixture]
public class TickControllerTests
{
    private Mock<ILogger<TickController>> _logger;
    private Mock<ITickService> _tickService;
    private Mock<ITickRepository> _tickRepository;
    private TickController _tickController;

    [SetUp]
    public void Setup()
    {
        _tickService = new Mock<ITickService>();
        _tickController = new TickController(_logger.Object, _tickService.Object, _tickRepository.Object);
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful()
    {
        //Arrange
        const int tickId = 1;
        _tickService
            .Setup(s => s.DeleteAsync(tickId));
        
        //Act
        var result = await _tickController.DeleteTick(tickId);
        //Assert
        Assert.That(result, Is.InstanceOf(typeof(NoContentResult)));
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenDeletionIsNotSuccessful()
    {
        //Arrange
        const int tickId = 999999;
        _tickService
            .Setup(s => s.DeleteAsync(tickId));
        
        //Act
        var result = await _tickController.DeleteTick(tickId);
        //Assert
        Assert.That(result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }
}