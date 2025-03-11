using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Extensions;

namespace RouteTickrAPI.Services;

public class TickService : ITickService
{
    private readonly ITickRepository _tickRepository;
    private readonly IClimbService _climbService;

    public TickService(ITickRepository tickRepository, IClimbService climbService)
    {
        _tickRepository = tickRepository;
        _climbService = climbService;
    }
    public async Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync()
    {
        try
        {
            var ticks = await _tickRepository.GetAllAsync();
            var tickDtoList = ticks.Select(TickDtoExtensions.ToTickDto).ToList();
            
            return tickDtoList.Count == 0 
                ? ServiceResult<IEnumerable<TickDto>>.NotFoundResult("No ticks found.") 
                : ServiceResult<IEnumerable<TickDto>>.SuccessResult(tickDtoList);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"ArgumentNullException in GetAllAsync: {e.Message}");
            return ServiceResult<IEnumerable<TickDto>>.ErrorResult($"A null value was encountered while mapping ticks, {e.Message}");
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
            if (id <= 0)
                throw new ArgumentException("ID must be greater than zero.");

            var tick = await _tickRepository.GetByIdAsync(id);
            if (tick is null)
                return ServiceResult<TickDto>.NotFoundResult($"Tick with ID: {id} not found.");

            var tickDto = tick.ToTickDto();

            return ServiceResult<TickDto>.SuccessResult(tickDto);
        }
        catch (ArgumentException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"Error: {e.Message}");
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
            if (tickIds.Count == 0)
                throw new ArgumentException("ID list is empty");

            var tickDtos = new List<TickDto>();

            foreach (var id in tickIds)
            {
                var tick = await _tickRepository.GetByIdAsync(id);
                if (tick is null) continue;
                var tickDto = tick.ToTickDto();
                tickDtos.Add(tickDto);
            }

            return tickDtos.Count == 0
                ? ServiceResult<List<TickDto>>.NotFoundResult("No Ticks found")
                : ServiceResult<List<TickDto>>.SuccessResult(tickDtos);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<List<TickDto>>.ErrorResult($"An argument was null, {e.Message}");
        }
        catch (ArgumentException e)
        {
            return ServiceResult<List<TickDto>>.ErrorResult($"Invalid parameter, {e.Message}");
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
            var result = await SaveTickAsync(tickDto);
            return !result.Success
                ? ServiceResult<TickDto>.ErrorResult("Error adding tick.")
                : ServiceResult<TickDto>.SuccessResult(tickDto);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"An argument was null {e.Message}");
        }
        catch (InvalidOperationException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"Issue saving tick {e.Message}");
        }
        catch (DbUpdateException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"Database error {e.Message}");
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
            await UpdateTickAsync(tickDto);

            return ServiceResult<TickDto>.SuccessResult(tickDto);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"An argument was null {e.Message}");
        }
        catch (InvalidOperationException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"Error updating tick {e.Message}");
        }
        catch (DbUpdateException e)
        {
            return ServiceResult<TickDto>.ErrorResult($"Database error {e.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            return ServiceResult<TickDto>.ErrorResult("An unexpected error occurred.");
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

    private async Task<Climb> GetOrSaveClimb(TickDto tickDto)
    {
        if (tickDto.Climb is null)
            throw new ArgumentNullException(nameof(tickDto));

        var result = await _climbService.AddClimbIfNotExists(tickDto.Climb);

        if (!result.Success || result.Data is null)
            throw new InvalidOperationException($"Failed to get or save climb: {result.ErrorMessage}");
        
        return result.Data;
    }

    public async Task<ServiceResult<TickDto>> SaveTickAsync(TickDto tickDto) //make private?
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));
        
        var climb = await GetOrSaveClimb(tickDto);
        var tick = tickDto.ToTickEntity(climb);
        
        var recordsAdded = await _tickRepository.AddAsync(tick);
        var tickDtoToReturn = tick.ToTickDto();

        return recordsAdded == 1
            ? ServiceResult<TickDto>.SuccessResult(tickDtoToReturn)
            : throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsAdded}");
    }

    public async Task<ServiceResult<TickDto>> SaveTickAsync(Tick tick)
    {
        ArgumentNullException.ThrowIfNull(tick, nameof(tick));
        
        var recordsAdded = await _tickRepository.AddAsync(tick);
        var tickDto = tick.ToTickDto();

        return recordsAdded == 1
            ? ServiceResult<TickDto>.SuccessResult(tickDto)
            : throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsAdded}");
    }

    private async Task UpdateTickAsync(TickDto tickDto)
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));
        
        var existingTick = await _tickRepository.GetByIdAsync(tickDto.Id);

        if (existingTick is null)
            throw new InvalidOperationException($"Tick with ID: {tickDto.Id} does not exist");

        var climb = tickDto.BuildClimb();
        var tickToUpdate = tickDto.ToTickEntity(climb);

        var recordsUpdated = await _tickRepository.UpdateAsync(existingTick, tickToUpdate);

        if (recordsUpdated != 1) 
            throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsUpdated}");
    }
    
}