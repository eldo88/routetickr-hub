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
            var tickDtoList = ticks.Select(TickMapper.ToTickDto).ToList();
            return tickDtoList.Count == 0 
                ? ServiceResult<IEnumerable<TickDto>>.ErrorResult("No ticks found.") 
                : ServiceResult<IEnumerable<TickDto>>.SuccessResult(tickDtoList);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetAllAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<TickDto>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0) return ServiceResult<TickDto>.ErrorResult("ID must be greater than zero.");
            var tick = await _tickRepository.GetByIdAsync(id);
            if (tick is null) return ServiceResult<TickDto>.ErrorResult($"Tick with ID: {id} not found.");
            var tickDto = TickMapper.ToTickDto(tick);
            return ServiceResult<TickDto>.SuccessResult(tickDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetByIdAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds)
    {
        try
        {
            if (tickIds.Count == 0) return ServiceResult<List<TickDto>>.ErrorResult("ID list is empty");
            var tickDtos = new List<TickDto>();
            foreach (var id in tickIds)
            {
                var tick = await _tickRepository.GetByIdAsync(id);
                if (tick is null) continue;
                var tickDto = TickMapper.ToTickDto(tick);
                tickDtos.Add(tickDto);
            }

            return tickDtos.Count == 0
                ? ServiceResult<List<TickDto>>.ErrorResult("No Ticks found")
                : ServiceResult<List<TickDto>>.SuccessResult(tickDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResult<TickDto>> AddAsync(TickDto tickDto)
    {
        try
        {
            var tick = TickMapper.ToTick(tickDto);
            var isTickAdded = await _tickRepository.AddAsync(tick);
            if (!isTickAdded) return ServiceResult<TickDto>.ErrorResult("Error adding tick.");
            var tickAdded = TickMapper.ToTickDto(tick);
            return ServiceResult<TickDto>.SuccessResult(tickAdded);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in AddAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<TickDto>> UpdateAsync(TickDto tickDto)
    {
        try
        {
            var recordToBeUpdated = await GetByIdAsync(tickDto.Id); //TODO use method in this class or repository?
            if (!recordToBeUpdated.Success) return recordToBeUpdated;
            var tick = TickMapper.ToTick(tickDto);
            var isUpdated = await _tickRepository.UpdateAsync(tick);
            return isUpdated 
                ? ServiceResult<TickDto>.SuccessResult(tickDto)
                : ServiceResult<TickDto>.ErrorResult($"Error updating tick with ID: {tickDto.Id}");
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
            return isDeleted 
                ? ServiceResult<bool>.SuccessResult(true) 
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
            var savedTickIds = new List<int>();
            foreach (var tick in dataFromFile.Select(TickMapper.ToTick))
            {
                var tickAdded = await _tickRepository.AddAsync(tick); //TODO use method in this class?
                if (!tickAdded)
                {
                    await RollbackDatabaseAsync(savedTickIds);
                    return ServiceResult<bool>.ErrorResult("Error uploading file contents, no data was saved.");
                }
                savedTickIds.Add(tick.Id);
            }

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error is ImportFileAsync: {e.Message}");
            throw;
        }
    }

    private async Task RollbackDatabaseAsync(IEnumerable<int> tickIds)
    {
        foreach (var tickId in tickIds)
        {
            await _tickRepository.DeleteAsync(tickId);
        }
    }
    
}