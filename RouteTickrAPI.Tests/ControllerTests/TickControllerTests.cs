using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteTickrAPI.Controllers;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Tests;

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
        var tickId = 1;
        _tickService
            .Setup(s => s.DeleteAsync(tickId))
            .ReturnsAsync(ServiceResult<bool>.SuccessResult(true));
        //Act
        var result = await _tickController.Delete(tickId);
        //Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }
}