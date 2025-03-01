using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Enums;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;
using RouteTickrAPI.Tests.TestHelpers;

namespace RouteTickrAPI.Tests.ServiceTests;

public class ImportFileServiceTests
{
    private IImportFileService _importFileService;
    private Mock<ITickRepository> _tickRepository;
    private Mock<IDbContextTransaction> _transactionMock;
    private Mock<IClimbService> _climbService;
    
    [SetUp]
    public void Setup()
    {
        _tickRepository = new Mock<ITickRepository>();
        _transactionMock = new Mock<IDbContextTransaction>();
        _climbService = new Mock<IClimbService>();
        _importFileService = new ImportFileService(_tickRepository.Object, _climbService.Object);
    }
    
    [Test]
    public async Task ImportFileAsync_SuccessfulImport_CommitsTransaction()
    {
        //Arrange
        var formFile = ImportFileHelper.CreateMockImportFileWithMultipleTicks();
        var fileDto = await formFile.ToImportFileDto();

        _tickRepository
            .Setup(r => r.BeginTransactionAsync())
            .ReturnsAsync(_transactionMock.Object);

        var climb = new SportRoute()
        {
            ClimbType = ClimbType.Sport,
            DangerRating = ClimbDangerRating.G,
            Height = 50,
            Id = 0,
            Location = "Rifle",
            Name = "Baby Brother",
            NumberOfBolts = 7,
            NumberOfPitches = 1,
            Rating = "5.10d",
            Url = "https://example.com/tick"
        };

        _climbService
            .Setup(cs => cs.AddClimbIfNotExists(It.IsAny<Climb>()))
            .ReturnsAsync(ServiceResult<Climb>.SuccessResult(climb));

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ReturnsAsync(true);
        //Act
        var result = await _importFileService.ImportFileAsync(fileDto);
        
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
        var fileDto = await formFile.ToImportFileDto();

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
        var result = await _importFileService.ImportFileAsync(fileDto);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred. Please try again."));
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
        var fileDto = await formFile.ToImportFileDto();
        
        _tickRepository
            .Setup(r => r.BeginTransactionAsync())
            .ReturnsAsync(_transactionMock.Object);

        _tickRepository
            .Setup(r => r.AddAsync(It.IsAny<Tick>()))
            .ThrowsAsync(new Exception("Database exception"));
        //Act
        var result = await _importFileService.ImportFileAsync(fileDto);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred. Please try again."));
            Assert.That(result.Data, Is.False);
            
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once, "Rollback should be called when an exception is thrown.");
        });
    }
}