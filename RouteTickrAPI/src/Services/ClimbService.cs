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
        var result = await _climbRepository.GetByIdAsync(climb.Id);
        if (result is not null) 
            return ServiceResult<Climb>.SuccessResult(climb);
        
        var savedClimb = await _climbRepository.AddClimb(climb);
        
        return savedClimb == 2
            ? ServiceResult<Climb>.SuccessResult(climb)
            : ServiceResult<Climb>.ErrorResult("Error saving climb.");

    }
}