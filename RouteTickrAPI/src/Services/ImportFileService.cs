using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Mappers;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public class ImportFileService : IImportFileService
{
    
    private readonly ITickRepository _tickRepository;
    private readonly IClimbService _climbService;
    private readonly ITickService _tickService;
    private readonly IPublisherService _publisherService;

    public ImportFileService(ITickRepository tickRepository, IClimbService climbService, ITickService tickService, IPublisherService publisherServiceService)
    {
        _tickRepository = tickRepository;
        _climbService = climbService;
        _tickService = tickService;
        _publisherService = publisherServiceService;
    }

    public async Task<ServiceResult<int>> ProcessFile(ImportFileDto fileDto, string userId)
    {
        try
        {
            using var stream = new StringReader(fileDto.Content);
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));
            var dataFromFile = ConvertCsvFileToTickDto(csvFile);

            var ticksSaved = await SaveFileContentsAsync(dataFromFile, userId);
            if (ticksSaved > 0)
            {
                await PublishUrls(dataFromFile);
            }
            
            return ServiceResult<int>.SuccessResult(dataFromFile.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<int> SaveFileContentsAsync(List<TickDto> dataFromFile, string userId)
    {
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            var count = 0;
            foreach (var tickDto in dataFromFile)
            {
                tickDto.UserId = userId;
                await SaveClimbAsync(tickDto);
                await SaveTickAsync(tickDto);
                count++;
            }

            if (dataFromFile.Count != count)
            {
                throw new InvalidOperationException("Error saving file contents.");
            }

            await _tickRepository.CommitTransactionAsync(transaction);
            return count;
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                await _tickRepository.RollbackTransactionAsync(transaction);
            }
            throw;
        }
    }

    private static List<TickDto> ConvertCsvFileToTickDto(CsvReader? csvFile)
    {
        ArgumentNullException.ThrowIfNull(csvFile, nameof(csvFile));
        
        csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
        return csvFile.GetRecords<TickDto>().ToList();
    }

    private async Task PublishUrls(List<TickDto> dtos)
    {
        try
        {
            await _publisherService.InitializeAsync();
            foreach (var dto in dtos)
            {
                await _publisherService.PublishMessageAsync(dto.Url);
            }
        }
        catch (Exception e)
        { //Swallow exception, background service that doesn't impact user TODO add logging
            Console.WriteLine(e);
        }
    }

    private async Task SaveClimbAsync(TickDto tickDto)
    {
        var climb = tickDto.BuildClimb();
        var result = await _climbService.GetOrSaveClimb(climb);
        tickDto.Climb = result;
    }

    private async Task SaveTickAsync(TickDto tickDto)
    {
        await _tickService.AddAsync(tickDto);
    }
}