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

    public async Task<int> ProcessFile(ImportFileDto fileDto, string userId, IDbContextTransaction? transaction = null)
    {
        try
        {
            using var stream = new StringReader(fileDto.Content);
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));
            var dataFromFile = ConvertCsvFileToTickDto(csvFile);

            var ticksSaved = await SaveFileContentsAsync(dataFromFile, userId, transaction);
            if (ticksSaved > 0)
            {
                await PublishUrls(dataFromFile);
            }

            return ticksSaved;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<int> SaveFileContentsAsync(List<TickDto> dataFromFile, string userId, IDbContextTransaction? transaction = null)
    {
        try
        {
            foreach (var tickDto in dataFromFile)
            {
                tickDto.UserId = userId;
                var climb = tickDto.BuildClimb();
                var result = await _climbService.GetOrSaveClimb(climb);
                tickDto.Climb = result;
                await _tickService.AddAsync(tickDto);
            }
            
            return dataFromFile.Count;
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
}