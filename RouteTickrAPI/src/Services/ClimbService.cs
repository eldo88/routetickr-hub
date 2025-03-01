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
            
            return climbDtoList.Count == 0
                ? ServiceResult<IEnumerable<ClimbDto>>.ErrorResult("No climbs found")
                : ServiceResult<IEnumerable<ClimbDto>>.SuccessResult(climbDtoList);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"ArgumentNullException in GetAllAsync: {ex.Message}");
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult("A null value was encountered while mapping climbs.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            return ServiceResult<IEnumerable<ClimbDto>>.ErrorResult("An unexpected error occurred.");
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

            var result = await AddAsync(climb);

            return result.Success
                ? ServiceResult<Climb>.SuccessResult(climb)
                : ServiceResult<Climb>.ErrorResult("Error saving climb.");
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"ArgumentNullException occurred in {e.TargetSite} {e.Message} {e.StackTrace}");
            return ServiceResult<Climb>.ErrorResult($"Error occurred because {e.ParamName} is null or empty.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResult<Climb>> GetByIdAsync(int id)
    {
        var result = await _climbRepository.GetByIdAsync(id);

        return result is not null
            ? ServiceResult<Climb>.SuccessResult(result)
            : ServiceResult<Climb>.NotFoundResult("Climb not found");
    }

    public async Task<ServiceResult<Climb>> AddAsync(Climb climb)
    {
        try
        {
            var recordsWritten = await _climbRepository.AddClimb(climb);

            return recordsWritten == 2
                ? ServiceResult<Climb>.SuccessResult(climb)
                : ServiceResult<Climb>.ErrorResult($"Error saving climb {climb.Name}.");
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
            : ServiceResult<Climb>.NotFoundResult("No climb found with name and location");
    }
}