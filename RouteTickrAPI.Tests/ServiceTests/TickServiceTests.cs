using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
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
    private Mock<IDbContextTransaction> _transactionMock;

    [SetUp]
    public void Setup()
    {
        _tickRepository = new Mock<ITickRepository>();
        _tickService = new TickService(_tickRepository.Object);
        _transactionMock = new Mock<IDbContextTransaction>();
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
        
        var expectedDtoList = mockTicks.Select(TickMapper.ToTickDto).ToList();
        
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
    public async Task GetByIdAsync_ReturnsError_WhenIdIsZeroOrNegative()
    {
        //Arrange
        var invalidIds = new[] { 0, -1, -100 };

        foreach (var id in invalidIds)
        {
            //Act
            var result = await _tickService.GetByIdAsync(id);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.ErrorMessage, Is.EqualTo("ID must be greater than zero."));
            });
        }
    }

    [Test]
    public async Task GetByIdAsync_ReturnsError_WhenTickNotFound()
    {
        //Arrange
        const int tickId = 9999;

        _tickRepository
            .Setup(r => r.GetByIdAsync(tickId))
            .ReturnsAsync((Tick?)null);
        //Act
        var result = await _tickService.GetByIdAsync(tickId);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo($"Tick with ID: {tickId} not found."));
        });
    }

    [Test]
    public async Task GetByIdAsync_ReturnsTick_WhenTickExists()
    {
        //Arrange
        var tick = TickBuilder.CreateValidTick();
        tick.Id = 55; // needed because id is defaulted to 0 in TickBuilder.CreateValidTick()

        _tickRepository
            .Setup(r => r.GetByIdAsync(tick.Id))
            .ReturnsAsync(tick);
        //Act
        var result = await _tickService.GetByIdAsync(tick.Id);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo(tick.Id));
        });
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsError_WhenExceptionIsThrown()
    {
        // Arrange
        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database failure"));
        // Act
        var result = await _tickService.GetByIdAsync(1);
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        });
    }

    [Test]
    public async Task GetByListOfIdsAsync_ReturnsError_WhenListOfIdsIsEmpty()
    {
        //Arrange
        var emptyList = new List<int>();
        //Act
        var result = await _tickService.GetByListOfIdsAsync(emptyList);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("ID list is empty"));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task GetByListOfIdsAsync_ReturnsError_WhenIdsNotFoundInRepository()
    {
        //Arrange
        var listOfInvalidTickIds = new List<int>() { 9999, 55555 };
        //Act
        var result = await _tickService.GetByListOfIdsAsync(listOfInvalidTickIds);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No Ticks found"));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task GetByListOfIdsAsync_ReturnsError_WhenExceptionIsThrown()
    {
        //Arrange
        var tickIds = new List<int>() { 1, 2 };
        
        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database failure"));
        //Act
        var result = await _tickService.GetByListOfIdsAsync(tickIds);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        });
    }

    [Test]
    public async Task GetByListOfIdsAsync_ReturnsListOfTickDtos_WhenIdsExistInRepository()
    {
        //Arrange
        var tick1 = TickBuilder.CreateValidTick();
        tick1.Id = 1;
        var tick2 = TickBuilder.CreateValidTick();
        tick2.Id = 2;
        
        var ticks = new List<Tick>() { tick1, tick2 };
        var ids = new List<int>() { 1, 2 };

        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => ticks.FirstOrDefault(t => t.Id == id));
        //Act
        var result = await _tickService.GetByListOfIdsAsync(ids);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data.Count, Is.EqualTo(2));
            Assert.That(result.Data[0].Id, Is.EqualTo(1));
            Assert.That(result.Data[1].Id, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task AddAsync_ReturnsError_WhenTickNotAdded()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(false);
        //Act
        var result = await _tickService.AddAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Error adding tick."));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task AddAsync_ReturnsError_WhenExceptionIsThrown()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ThrowsAsync(new Exception("Database failure"));
        //Act
        var result = await _tickService.AddAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        });
    }

    [Test]
    public async Task AddAsync_ReturnsSuccess_WhenTickIsAdded()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(true);
        //Act
        var result = await _tickService.AddAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Data, Is.Not.Null);
        });
    }
    
    [Test]
    public async Task AddAsync_ReturnsError_WhenTickDtoIsNull()
    {
        //Act
        var result = await _tickService.AddAsync(null);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
        });
    }

    [Test]
    public async Task AddAsync_ReturnsCorrectTickDto_WhenTickIsAdded()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(true);
        //Act
        var result = await _tickService.AddAsync(tickDto);
        //Assert
        Assert.That(result.Data, Is.EqualTo(tickDto).Using(new TickDtoComparer()));
    }

    [Test]
    public async Task UpdateAsync_ReturnsError_WhenTickNotFound()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Tick?)null);
        //Act
        var result = await _tickService.UpdateAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo($"Tick with ID: {tickDto.Id} does not exist"));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task UpdateAsync_ReturnsError_WhenUpdateFails()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();
        var tick = TickMapper.ToTick(tickDto);

        _tickRepository
            .Setup(r => r.GetByIdAsync(tickDto.Id))
            .ReturnsAsync(tick);

        _tickRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Tick>()))
            .ReturnsAsync(false);
        //Act
        var result = await _tickService.UpdateAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo($"Error updating tick with ID: {tickDto.Id}"));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task UpdateAsync_ReturnsError_WhenExceptionIsThrownByGetByIdAsync()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();

        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database failure"));
        
        //Act
        var result = await _tickService.UpdateAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task UpdateAsync_ReturnsError_WhenExceptionIsThrownByUpdateAsync()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();
        var tick = TickMapper.ToTick(tickDto);

        _tickRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(tick);

        _tickRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Tick>()))
            .ThrowsAsync(new Exception("Database failure"));
        //Act
        var result = await _tickService.UpdateAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public async Task UpdateAsync_ReturnsSuccess_WhenTickExistsAndIsUpdated()
    {
        //Arrange
        var tickDto = TickBuilder.CreateValidTickDto();
        tickDto.Id = 1;
        var tick = TickMapper.ToTick(tickDto);
        
        _tickRepository
            .Setup(r => r.GetByIdAsync(tickDto.Id))
            .ReturnsAsync(tick);

        _tickRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Tick>()))
            .ReturnsAsync(true);
        //Act
        var result = await _tickService.UpdateAsync(tickDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Id, Is.EqualTo(tickDto.Id));
        });
    }

    [Test]
    public async Task ImportFileAsync_SuccessfulImport_CommitsTransaction()
    {
        //Arrange
        var formFile = ImportFileHelper.CreateMockImportFileWithMultipleTicks();

        _tickRepository
            .Setup(r => r.BeginTransactionAsync())
            .ReturnsAsync(_transactionMock.Object);

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(true);
        //Act
        var result = await _tickService.ImportFileAsync(formFile);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Data, Is.True);
       
            _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once, "The transaction should be committed.");
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never, "Rollback should not be called on a successful import.");
        });
    }

    [Test]
    public async Task ImportFileAsync_TickAdditionFails_RollsBackTransactionAndReturnsError()
    {
        //Arrange
        var formFile = ImportFileHelper.CreateMockImportFileWithMultipleTicks();

        _tickRepository
            .Setup(r => r.BeginTransactionAsync())
            .ReturnsAsync(_transactionMock.Object);

        var callCount = 0;
        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1;
            });
        //Act
        var result = await _tickService.ImportFileAsync(formFile);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Error uploading file contents, no data was saved."));
            Assert.That(result.Data, Is.False);
            
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once, "A rollback should occur when a tick fails.");
            _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never, "Commit should not be called if any tick fails.");
        });
    }

    [Test]
    public async Task ImportFileAsync_ExceptionDuringProcessing_RollsBackTransactionAndReturnsError()
    {
        //Arrange
        var formFile = ImportFileHelper.CreateMockImportFileWithSingleTick();
        
        _tickRepository
            .Setup(r => r.BeginTransactionAsync())
            .ReturnsAsync(_transactionMock.Object);

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ThrowsAsync(new Exception("Database exception"));
        //Act
        var result = await _tickService.ImportFileAsync(formFile);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An error occurred during file import."));
            Assert.That(result.Data, Is.False);
            
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once, "Rollback should be called when an exception is thrown.");
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