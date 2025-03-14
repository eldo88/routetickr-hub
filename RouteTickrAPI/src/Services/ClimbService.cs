using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;
using ArgumentException = System.ArgumentException;

namespace RouteTickrAPI.Services;

public class ClimbService : IClimbService
{
    private readonly IClimbRepository _climbRepository;

    public ClimbService(IClimbRepository climbRepository)
    {
        _climbRepository = climbRepository;
    }
    public async Task<ServiceResult<IEnumerable<ClimbDto>>> GetAllAsync()
    {
        try
        {
            var climbs = await _climbRepository.GetAllAsync();
            var climbDtoList = climbs.Select(ClimbDtoExtensions.ToDto).ToList();

            return climbDtoList.Count > 0
                ? ServiceResult<IEnumerable<ClimbDto>>.SuccessResult(climbDtoList)
                : ServiceResult<IEnumerable<ClimbDto>>.NotFoundResult("No climbs found");
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult(
                $"A null value was encountered while mapping climbs. {e.Message}");
        }
        catch (Exception e)
        {
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult(
                $"An unexpected error occurred. {e.Message}");
        }
    }

    public async Task<ServiceResult<Climb>> AddClimbIfNotExists(Climb climb)
    {
        try
        {
            var getByIdResult = await GetClimbByIdAsync(climb.Id);
            if (getByIdResult is not null)
                return ServiceResult<Climb>.SuccessResult(getByIdResult);

            var getByNameAndLocationResult = await GetClimbByNameAndLocationIfExists(climb.Name, climb.Location);
            if (getByNameAndLocationResult is not null)
                return ServiceResult<Climb>.SuccessResult(getByNameAndLocationResult);

            return await AddAsync(climb);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<Climb>.ErrorResult($"Error: {e.Message}");
        }
        catch (ArgumentException e)
        {
            return ServiceResult<Climb>.ErrorResult($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            return ServiceResult<Climb>.ErrorResult($"An unexpected error occurred: {e.Message}");
        }
    }

    public async Task<ServiceResult<Climb>> GetByIdAsync(int id) // TODO create controller 
    {
        var result = await GetClimbByIdAsync(id);

        return result is not null
            ? ServiceResult<Climb>.SuccessResult(result)
            : ServiceResult<Climb>.NotFoundResult($"Climb not found with ID {id}");
    }

    public async Task<ServiceResult<Climb>> AddAsync(Climb climb) // TODO create controller
    {
        try
        {
            await AddClimbAsync(climb);

            return ServiceResult<Climb>.SuccessResult(climb);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<Climb>.ErrorResult(
                $"Error saving climb, unexpected null value. {e.Message}");
        }
        catch (DbUpdateException e)
        {
            return ServiceResult<Climb>.ErrorResult(
                $"Error saving {climb.Name}, {e.Message}");
        }
        catch (OperationCanceledException e)
        {
            return ServiceResult<Climb>.ErrorResult(
                $"Error saving {climb.Name}, save operation cancelled. {e.Message}");
        }
        catch (Exception e)
        {
            return ServiceResult<Climb>.ErrorResult(
                $"Unexpected error occurred, no data was saved. {e.Message}");
        }
    }
    
    
    //private methods
    private async Task<Climb?> GetClimbByNameAndLocationIfExists(string name, string location)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        ArgumentException.ThrowIfNullOrEmpty(location, nameof(location));
        
        return await _climbRepository.GetByNameAndLocationAsync(name, location);
    }

    private async Task<Climb?> GetClimbByIdAsync(int id)
    {
        if (id < 0)
            throw new ArgumentException(
                $"Error retrieving climb from database, ID: {id} is invalid.");

        return await _climbRepository.GetByIdAsync(id);
    }

    private async Task AddClimbAsync(Climb climb)
    {
        ArgumentNullException.ThrowIfNull(climb, nameof(climb));

        var recordsWritten = await _climbRepository.AddClimb(climb);

        if (recordsWritten != 2)
            throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsWritten}");
    }

    private async Task UpdateClimbAsync(ClimbDto climbDto) // TODO not used
    {
        ArgumentNullException.ThrowIfNull(climbDto, nameof(climbDto));
        
        var climb = climbDto.ToEntity();

        var existingClimb = await GetClimbByIdAsync(climb.Id);

        if (existingClimb is null)
            throw new InvalidOperationException(
                $"Climb with {climb.Id} does not exist and cannot be updated");

        var recordsUpdated = await _climbRepository.UpdateAsync(existingClimb, climb);
        
        if (recordsUpdated != 1) 
            throw new InvalidOperationException(
                $"Unexpected number of records updated. Expected 1 but got {recordsUpdated}");
    }

    private async Task DeleteClimbAsync(int id) // TODO not used
    {
        if (id <= 0)
            throw new ArgumentException($"Error deleting climb, ID: {id} is invalid.");

        var climbToBeDeleted = await GetClimbByIdAsync(id);

        if (climbToBeDeleted is null)
            throw new InvalidOperationException(
                $"Climb with {id} does not exist and cannot be deleted");

        var recordsDeleted = await _climbRepository.DeleteAsync(climbToBeDeleted);
        
        if (recordsDeleted != 1)
            throw new InvalidOperationException(
                $"Unexpected number of records deleted. Expected 1 but got {recordsDeleted}");
    }
}