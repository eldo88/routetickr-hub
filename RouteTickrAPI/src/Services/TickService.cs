using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.DTOs;
using RouteTickr.Entities;
using RouteTickrAPI.Extensions;
using ArgumentException = System.ArgumentException;

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
            return ServiceResult<IEnumerable<TickDto>>.ErrorResult($"A null value was encountered while mapping ticks, {e.Message}");
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<TickDto>>.ErrorResult("An unexpected error occurred.");
        }
    }

    public async Task<ServiceResult<TickDto>> GetByIdAsync(int id)
    {
        try
        {
            var tick = await GetTickByIdAsync(id);
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
            return ServiceResult<TickDto>.ErrorResult($"An unexpected error occurred. {e.Message}");
        }
    }

    public async Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds)
    {//possibly not needed
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
        catch (Exception e)
        {
            return ServiceResult<List<TickDto>>.ErrorResult($"An unexpected error occurred. {e.Message}");
        }
    }

    public async Task<ServiceResult<TickDto>> AddAsync(TickDto tickDto)
    {
        try
        {
            var result = await SaveTickAsync(tickDto);

            return ServiceResult<TickDto>.SuccessResult(result);
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
        catch (Exception e)
        {
            return ServiceResult<TickDto>.ErrorResult($"An unexpected error occurred. {e.Message}");
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
        catch (Exception e)
        {
            return ServiceResult<TickDto>.ErrorResult($"An unexpected error occurred. {e.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            await DeleteTickAsync(id);

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (ArgumentException e)
        {
            return ServiceResult<bool>.ErrorResult($"Error: {e.Message}");
        }
        catch (InvalidOperationException e)
        {
            return ServiceResult<bool>.ErrorResult($"Error: {e.Message}");
        }
        catch (DbUpdateException e)
        {
            return ServiceResult<bool>.ErrorResult($"Database error: {e.Message}");
        }
        catch (Exception e)
        {
            return ServiceResult<bool>.ErrorResult($"Unexpected error occurred: {e.Message}");
        }
    }
    
    public async Task<TickDto> SaveTickAsync(TickDto tickDto) 
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));
        
        var climb = await GetOrSaveClimb(tickDto);
        var tick = tickDto.ToTickEntity(climb);
        
        var recordsAdded = await _tickRepository.AddAsync(tick);
        var tickDtoToReturn = tick.ToTickDto();

        return recordsAdded == 1
            ? tickDtoToReturn
            : throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsAdded}");
    }
    
    
    /*****************************************************************************************************/
    /* Private methods*/
    
    
    private async Task<Climb> GetOrSaveClimb(TickDto tickDto)
    {
        if (tickDto.Climb is null)
            throw new ArgumentNullException(nameof(tickDto));

        var result = await _climbService.GetOrSaveClimb(tickDto.Climb);
        
        return result;
    }

    private async Task UpdateTickAsync(TickDto tickDto)
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));
        
        var existingTick = await GetTickByIdAsync(tickDto.Id);

        if (existingTick is null)
            throw new InvalidOperationException(
                $"Tick with {tickDto.Id} does not exist and cannot be updated.");

        var climb = tickDto.BuildClimb();
        var updateTo = tickDto.ToTickEntity(climb);

        var recordsUpdated = await _tickRepository.UpdateAsync(existingTick, updateTo);

        if (recordsUpdated != 1) 
            throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsUpdated}");
    }

    private async Task DeleteTickAsync(int id)
    {
        var tickToBeDeleted = await GetTickByIdAsync(id);

        if (tickToBeDeleted is null)
            throw new InvalidOperationException(
                $"Tick with {id} does not exist and cannot be deleted.");

        var recordsDeleted = await _tickRepository.DeleteAsync(tickToBeDeleted);

        if (recordsDeleted != 1)
            throw new InvalidOperationException(
                $"Unexpected number of records deleted. Expected 1 but got {recordsDeleted}");
    }

    private async Task<Tick?> GetTickByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"ID: {id} is invalid.");
        
        return await _tickRepository.GetByIdAsync(id);
    }
    
}