using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;
using RouteTickrAPI.Tests.TestHelpers;

namespace RouteTickrAPI.Tests.ServiceTests;

public class ImportFileServiceTests
{
    private IImportFileService _importFileService;
    private Mock<ITickRepository> _tickRepository;
    private Mock<IDbContextTransaction> _transactionMock;
    
    [SetUp]
    public void Setup()
    {
        _tickRepository = new Mock<ITickRepository>();
        _transactionMock = new Mock<IDbContextTransaction>();
        _importFileService = new ImportFileService(_tickRepository.Object);
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
        var result = await _importFileService.ImportFileAsync(formFile);
        
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
        var result = await _importFileService.ImportFileAsync(formFile);
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
        var result = await _importFileService.ImportFileAsync(formFile);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An error occurred during file import."));
            Assert.That(result.Data, Is.False);
            
            _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once, "Rollback should be called when an exception is thrown.");
        });
    }
}