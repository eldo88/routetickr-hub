using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;
using ArgumentException = System.ArgumentException;

namespace RouteTickrAPI.Services;

public class ClimbService : IClimbService // TODO create controller for crud operations
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

    public async Task<ServiceResult<ClimbDto>> GetByIdAsync(int id) // create controller 
    {
        var result = await GetClimbByIdAsync(id);
        if (result is null) return ServiceResult<ClimbDto>.NotFoundResult($"Climb not found with ID {id}");
        var dto = result.ToDto();
        return ServiceResult<ClimbDto>.SuccessResult(dto);
    }

    public async Task<ServiceResult<ClimbDto>> AddAsync(ClimbDto climbDto) // create controller 
    {
        try
        {
            await AddClimbAsync(climbDto);

            return ServiceResult<ClimbDto>.SuccessResult(climbDto);
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<ClimbDto>.ErrorResult(
                $"Error saving climb, unexpected null value. {e.Message}");
        }
        catch (DbUpdateException e)
        {
            return ServiceResult<ClimbDto>.ErrorResult(
                $"Error saving {climbDto.Name}, {e.Message}");
        }
        catch (OperationCanceledException e)
        {
            return ServiceResult<ClimbDto>.ErrorResult(
                $"Error saving {climbDto.Name}, save operation cancelled. {e.Message}");
        }
        catch (Exception e)
        {
            return ServiceResult<ClimbDto>.ErrorResult(
                $"Unexpected error occurred, no data was saved. {e.Message}");
        }
    }

    public async Task<ServiceResult<ClimbDto>> UpdateAsync(ClimbDto dto) // create controller and implement method
    {
        try
        {
            await UpdateClimbAsync(dto);

            return ServiceResult<ClimbDto>.SuccessResult(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id) // create controller and implement method
    {
        try
        {
            await DeleteClimbAsync(id);

            return ServiceResult<bool>.SuccessResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Climb> GetOrSaveClimb(Climb climb)
    {
        try
        {
            var getByIdResult = await GetClimbByIdAsync(climb.Id);
            if (getByIdResult is not null)
                return getByIdResult;

            var getByNameAndLocationResult = await GetClimbByNameAndLocationIfExists(climb.Name, climb.Location);
            if (getByNameAndLocationResult is not null)
                return getByNameAndLocationResult;
            
            var climbDto = climb.ToDto();

            await AddClimbAsync(climbDto);

            return climb;
        }
        catch (ArgumentNullException e)
        {
            throw;
        }
        catch (ArgumentException e)
        {
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            throw;
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

    private async Task AddClimbAsync(ClimbDto climbDto)
    {
        ArgumentNullException.ThrowIfNull(climbDto, nameof(climbDto));

        var climb = climbDto.ToEntity();

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