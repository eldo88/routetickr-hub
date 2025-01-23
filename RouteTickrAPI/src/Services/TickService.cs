using System.Globalization;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using RouteTickrAPI.CsvMapper;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Mappers;

namespace RouteTickrAPI.Services;

public class TickService : ITickService
{
    private readonly ITickRepository _tickRepository;

    public TickService(ITickRepository tickRepository)
    {
        _tickRepository = tickRepository;
    }

    public async Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync()
    {
        try
        {
            var ticks = await _tickRepository.GetAllAsync();
            var tickDto = ticks.ToList();
            return tickDto.Count == 0 ? ServiceResult<IEnumerable<TickDto>>.ErrorResult("No ticks found.") 
                : ServiceResult<IEnumerable<TickDto>>.SuccessResult(tickDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetAllAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<Tick>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return ServiceResult<Tick>.ErrorResult("ID must be greater than zero.");
            }
            var tick = await _tickRepository.GetByIdAsync(id);
            return tick is null ? ServiceResult<Tick>.ErrorResult($"Tick with ID: {id} not found.") 
                : ServiceResult<Tick>.SuccessResult(tick);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetByIdAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<Tick>> AddAsync(Tick tick)
    {
        try
        {
            var isAdded = await _tickRepository.AddAsync(tick);
            return isAdded ? ServiceResult<Tick>.SuccessResult(tick) 
                : ServiceResult<Tick>.ErrorResult("Error adding tick.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in AddAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<Tick>> UpdateAsync(Tick tick)
    {
        try
        {
            var recordToBeUpdated = await GetByIdAsync(tick.Id);
            if (!recordToBeUpdated.Success)
                return recordToBeUpdated;
            var isUpdated = await _tickRepository.UpdateAsync(tick);
            return isUpdated ? ServiceResult<Tick>.SuccessResult(tick)
                : ServiceResult<Tick>.ErrorResult($"Error updating tick with ID: {tick.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in UpdateAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var isDeleted = await _tickRepository.DeleteAsync(id);
            return isDeleted ? ServiceResult<bool>.SuccessResult(true) 
                : ServiceResult<bool>.ErrorResult($"Error deleting tick with ID: {id}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in DeleteAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<bool>> ImportFileAsync(IFormFile file)
    {
        try
        {
            using var stream = new StreamReader(file.OpenReadStream()) ;
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));

            csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
            var dataFromFile = csvFile.GetRecords<TickDto>().ToList();

            var recordIsAdded = false;
            foreach (var tick in dataFromFile.Select(TickMapper.ToTick))
            {
                recordIsAdded = await _tickRepository.AddAsync(tick);
            }

            return recordIsAdded ? ServiceResult<bool>.SuccessResult(true)
                : ServiceResult<bool>.ErrorResult("Error uploading file contents.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error is ImportFileAsync: {e.Message}");
            throw;
        }
    }
    
}