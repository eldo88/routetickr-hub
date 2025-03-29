using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteTickrAPI.Controllers;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Tests.ControllerTests;

[TestFixture]
public class TickControllerTests
{
    private Mock<ITickService> _tickService;
    private TickController _tickController;

    [SetUp]
    public void Setup()
    {
        _tickService = new Mock<ITickService>();
        _tickController = new TickController(_tickService.Object);
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful()
    {
        //Arrange
        const int tickId = 1;
        _tickService
            .Setup(s => s.DeleteAsync(tickId));
        
        //Act
        var result = await _tickController.Delete(tickId);
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
        var result = await _tickController.Delete(tickId);
        //Assert
        Assert.That(result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }
}