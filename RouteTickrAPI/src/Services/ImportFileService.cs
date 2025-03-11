using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
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

    public ImportFileService(ITickRepository tickRepository, IClimbService climbService, ITickService tickService)
    {
        _tickRepository = tickRepository;
        _climbService = climbService;
        _tickService = tickService;
    }
    public async Task<ServiceResult<bool>> ImportFileAsync(ImportFileDto fileDto)
    {
        await using var transaction = await _tickRepository.BeginTransactionAsync();
        try
        {
            using var stream = new StringReader(fileDto.Content);
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));

            var dataFromFile = ConvertCsvFileToTickDto(csvFile);

            foreach (var tickDto in dataFromFile)
            {
                await SaveClimbAsync(tickDto);
                await SaveTickAsync(tickDto);
            }

            await transaction.CommitAsync();
            
            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (CsvHelperException e)
        {
            Console.WriteLine($"CSV parsing error, {e.Message}");
            await transaction.RollbackAsync();
            return ServiceResult<bool>.ErrorResult($"Invalid CSV format in {fileDto.FileName}. Please check the file.");
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Database error, {e.StackTrace}");
            await transaction.RollbackAsync();
            return ServiceResult<bool>.ErrorResult("Database error occurred while saving ticks.");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Error in {e.TargetSite}, {e.StackTrace}");
            await transaction.RollbackAsync();
            return ServiceResult<bool>.ErrorResult(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error is ImportFileAsync: {e.StackTrace}");
            await transaction.RollbackAsync();
            return ServiceResult<bool>.ErrorResult($"An unexpected error occurred. {e.Message}");
        }
    }

    private static List<TickDto> ConvertCsvFileToTickDto(CsvReader? csvFile)
    {
        ArgumentNullException.ThrowIfNull(csvFile, nameof(csvFile));
        
        csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
        return csvFile.GetRecords<TickDto>().ToList();
    }

    private async Task SaveClimbAsync(TickDto tickDto)
    {
        var route = tickDto.BuildClimb();
        var result = await _climbService.AddClimbIfNotExists(route);
        if (!result.Success) 
            throw new InvalidOperationException($"Failed to save climb. {result.ErrorMessage}");
        tickDto.Climb = result.Data;
    }

    private async Task SaveTickAsync(TickDto tickDto)
    {
        var tick = tickDto.ToEntity();
        var result = await _tickService.SaveTickAsync(tick);
        if (!result.Success)
            throw new InvalidOperationException($"Failed to save tick. {result.ErrorMessage}");
    }
}