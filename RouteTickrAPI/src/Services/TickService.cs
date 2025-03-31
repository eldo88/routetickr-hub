using Microsoft.EntityFrameworkCore.Storage;
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
    public async Task<IEnumerable<TickDto>> GetAllAsync()
    {
        var ticks = await _tickRepository.GetAllAsync();
        return ticks.Select(TickDtoExtensions.ToTickDto).ToList();
    }

    public async Task<TickDto?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"ID: {id} is invalid.");
        
        var tick = await _tickRepository.GetByIdAsync(id);
        
        return tick?.ToTickDto();
    }

    public async Task<List<TickDto>> GetByListOfIdsAsync(List<int> tickIds)
    {//possibly not needed
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

        return tickDtos;
    }

    public async Task<TickDto> AddAsync(TickDto tickDto)
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));

        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            
            var climb = await GetOrSaveClimb(tickDto);
            var tick = tickDto.ToTickEntity(climb);
        
            var recordsAdded = await _tickRepository.AddAsync(tick);
            var tickDtoToReturn = tick.ToTickDto();

            if (recordsAdded != 1)
            {
                throw new InvalidOperationException(
                    $"Unexpected number of records saved. Expected 1 but got {recordsAdded}");
            }
            
            await _tickRepository.CommitTransactionAsync(transaction);

            return tickDtoToReturn;
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

    public async Task<TickDto> UpdateAsync(TickDto tickDto)
    {
        ArgumentNullException.ThrowIfNull(tickDto, nameof(tickDto));

        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            
            var existingTick = await GetByIdAsync(tickDto.Id);

            if (existingTick is null)
                throw new InvalidOperationException(
                    $"Tick with ID: {tickDto.Id} does not exist and cannot be updated.");

            var climb = tickDto.BuildClimb();
            var updateTo = tickDto.ToTickEntity(climb);
            var tick = existingTick.ToEntity();

            var recordsUpdated = await _tickRepository.UpdateAsync(tick, updateTo);

            if (recordsUpdated != 1) 
                throw new InvalidOperationException(
                    $"Unexpected number of records saved. Expected 1 but got {recordsUpdated}");

            await _tickRepository.CommitTransactionAsync(transaction);

            return tickDto;
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

    public async Task DeleteAsync(int id)
    {
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            
            var tick = await GetByIdAsync(id);

            if (tick is null)
                throw new InvalidOperationException(
                    $"Tick with ID: {id} does not exist and cannot be deleted.");
        
            var tickToBeDeleted = tick.ToEntity();

            var recordsDeleted = await _tickRepository.DeleteAsync(tickToBeDeleted);

            if (recordsDeleted != 1)
                throw new InvalidOperationException(
                    $"Unexpected number of records deleted. Expected 1 but got {recordsDeleted}");

            await _tickRepository.CommitTransactionAsync(transaction);
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
            ArgumentNullException.ThrowIfNull(tickDto.Climb);

        var result = await _climbService.GetOrSaveClimb(tickDto.Climb);
        
        return result;
    }
}