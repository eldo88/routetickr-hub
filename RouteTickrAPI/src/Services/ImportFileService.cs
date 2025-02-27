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

    public ImportFileService(ITickRepository tickRepository, IClimbService climbService)
    {
        _tickRepository = tickRepository;
        _climbService = climbService;
    }
    
    public async Task<ServiceResult<bool>> ImportFileAsync(IFormFile file)
    {
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            
            using var stream = new StreamReader(file.OpenReadStream()) ;
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));
            
            csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
            var dataFromFile = csvFile.GetRecords<TickDto>().ToList();

            foreach (var tickDto in dataFromFile)
            {
                var route = tickDto.BuildClimb();
                var result = await _climbService.AddClimbIfNotExists(route);
                tickDto.Climb = result.Data;
            }
            
            foreach (var tick in dataFromFile.Select(TickDtoExtensions.ToEntity))
            {
                var tickAdded = await _tickRepository.AddAsync(tick);
                if (tickAdded) continue;
                await transaction.RollbackAsync();
                
                return ServiceResult<bool>.ErrorResult("Error uploading file contents, no data was saved.");
            }

            await transaction.CommitAsync();
            
            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error is ImportFileAsync: {e.Message}");
            
            if (transaction is not null)
            {
                await transaction.RollbackAsync();
            }

            return ServiceResult<bool>.ErrorResult("An error occurred during file import.");
        }
    }
}