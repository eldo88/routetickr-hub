using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;

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
            Console.WriteLine($"ArgumentNullException in GetAllAsync: {e.Message}");
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult($"A null value was encountered while mapping climbs. {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetAllAsync: {e.Message}");
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult($"An unexpected error occurred. {e.Message}");
        }
    }

    public async Task<ServiceResult<Climb>> AddClimbIfNotExists(Climb climb)
    {
        try
        {
            var getByIdResult = await GetByIdAsync(climb.Id);
            if (getByIdResult.Success)
                return getByIdResult;

            var getByNameAndLocationResult = await GetClimbByNameAndLocationIfExists(climb.Name, climb.Location);
            if (getByNameAndLocationResult.Success)
                return getByNameAndLocationResult;

            return await AddAsync(climb);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"ArgumentNullException occurred in {e.TargetSite} {e.Message} {e.StackTrace}");
            return ServiceResult<Climb>.ErrorResult($"Error occurred because {e.ParamName} is null or empty.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ServiceResult<Climb>.ErrorResult($"An unexpected error occurred: {e.Message}");
        }
    }

    public async Task<ServiceResult<Climb>> GetByIdAsync(int id)
    {
        var result = await _climbRepository.GetByIdAsync(id);

        return result is not null
            ? ServiceResult<Climb>.SuccessResult(result)
            : ServiceResult<Climb>.NotFoundResult($"Climb not found with ID {id}");
    }

    public async Task<ServiceResult<Climb>> AddAsync(Climb climb)
    {
        try
        {
            var recordsWritten = await _climbRepository.AddClimb(climb);

            return recordsWritten == 2
                ? ServiceResult<Climb>.SuccessResult(climb)
                : ServiceResult<Climb>.ErrorResult($"Error saving climb {climb.Name}."); // throw exception?
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Exception occurred in {e.TargetSite} due to a database exception, " +
                              $"Message:  {e.Message}, " +
                              $"Stack Trace: {e.StackTrace}");
            
            return ServiceResult<Climb>.ErrorResult($"Error saving {climb.Name}, {e.Message}");
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine($"Exception occurred in {e.TargetSite} due to operation being cancelled, " +
                              $"Message:  {e.Message}, " +
                              $"Stack Trace: {e.StackTrace}");
            
            return ServiceResult<Climb>.ErrorResult($"Error saving {climb.Name}, save operation cancelled." +
                                                    $"{e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred in {e.TargetSite}, " +
                              $"Message:  {e.Message}, " +
                              $"Stack Trace: {e.StackTrace}");
            
            return ServiceResult<Climb>.ErrorResult("Unexpected error occurred, no data was saved.");
        }
    }
    
    
    //private methods
    private async Task<ServiceResult<Climb>> GetClimbByNameAndLocationIfExists(string name, string location)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        
        if (string.IsNullOrEmpty(location)) throw new ArgumentNullException(nameof(location));
        
        var result = await _climbRepository.GetByNameAndLocationAsync(name, location);

        return result is not null
            ? ServiceResult<Climb>.SuccessResult(result)
            : ServiceResult<Climb>.NotFoundResult($"No climb found with {name} and {location}");
    }

    private async Task<Climb> GetClimbByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Error deleting climb, ID: {id} is invalid.");

        var climb = await _climbRepository.GetByIdAsync(id);
        
        return climb ?? throw new InvalidOperationException($"Climb with ID: {id} does not exist.");
    }

    private async Task UpdateClimbAsync(ClimbDto climbDto)
    {
        ArgumentNullException.ThrowIfNull(climbDto, nameof(climbDto));
        
        var climb = climbDto.ToEntity();

        var existingClimb = await GetClimbByIdAsync(climb.Id);

        var recordsUpdated = await _climbRepository.UpdateAsync(existingClimb, climb);
        
        if (recordsUpdated != 1) 
            throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsUpdated}");
    }

    private async Task DeleteClimbAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Error deleting climb, ID: {id} is invalid.");

        var climbToBeDeleted = await GetClimbByIdAsync(id);

        var recordsDeleted = await _climbRepository.DeleteAsync(climbToBeDeleted);
        
        if (recordsDeleted != 1)
            throw new InvalidOperationException(
                $"Unexpected number of records deleted. Expected 1 but got {recordsDeleted}");
    }
}