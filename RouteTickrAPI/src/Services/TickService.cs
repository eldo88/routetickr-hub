using System.Globalization;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
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
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"ArgumentNullException in GetAllAsync: {ex.Message}");
            return ServiceResult<IEnumerable<TickDto>>.ErrorResult("A null value was encountered while mapping ticks.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            return ServiceResult<IEnumerable<TickDto>>.ErrorResult("An unexpected error occurred.");
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
            return ServiceResult<TickDto>.ErrorResult("An unexpected error occurred.");
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured in getByListOfIdsAsync {ex.Message}");
            return ServiceResult<List<TickDto>>.ErrorResult("An unexpected error occurred.");
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            return ServiceResult<TickDto>.ErrorResult("An unexpected error occurred.");
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
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            using var stream = new StreamReader(file.OpenReadStream()) ;
            using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
            var dataFromFile = csvFile.GetRecords<TickDto>().ToList();
            
            foreach (var tick in dataFromFile.Select(TickMapper.ToTick))
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
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }

            return ServiceResult<bool>.ErrorResult("An error occurred during file import.");
        }
    }
    
}