using RouteTickrAPI.DTOs;
using RouteTickr.Entities;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;
using ArgumentException = System.ArgumentException;

namespace RouteTickrAPI.Services;

public class ClimbService : IClimbService // TODO create controller for crud operations
{
    private readonly ILogger<ClimbService> _logger;
    private readonly IClimbRepository _climbRepository;

    public ClimbService(ILogger<ClimbService> logger, IClimbRepository climbRepository)
    {
        _logger = logger;
        _climbRepository = climbRepository;
    }
    public async Task<IEnumerable<ClimbDto>> GetAllAsync()
    {
        var climbs = await _climbRepository.GetAllAsync();
        
        return climbs.Select(ClimbDtoExtensions.ToDto).ToList();
    }

    public async Task<ClimbDto?> GetByIdAsync(int id) // create controller 
    {
        var result = await GetClimbByIdAsync(id);
       
        return result?.ToDto();
    }

    public async Task<ClimbDto> AddAsync(ClimbDto climbDto) // create controller 
    {
        var climb = climbDto.ToEntity();
            
        await AddClimbAsync(climb);

        return climbDto;
    }

    public async Task<ClimbDto> UpdateAsync(ClimbDto dto) // create controller and implement method
    {
        await UpdateClimbAsync(dto);

        return dto;
    }

    public async Task DeleteAsync(int id) // create controller and implement method
    {
        await DeleteClimbAsync(id);
    }

    public async Task<Climb> GetOrSaveClimb(Climb climb)
    {
        var getByIdResult = await GetClimbByIdAsync(climb.Id);
        if (getByIdResult is not null)
            return getByIdResult;

        var getByNameAndLocationResult = await GetClimbByNameAndLocationIfExists(climb.Name, climb.Location);
        if (getByNameAndLocationResult is not null)
            return getByNameAndLocationResult;
        
        await AddClimbAsync(climb);

        return climb;
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
        {
            _logger.LogError($"{recordsWritten} number of records wrtting, should be 2");
            throw new InvalidOperationException(
                $"Unexpected number of records saved. Expected 1 but got {recordsWritten}");   
        }
    }

    private async Task UpdateClimbAsync(ClimbDto climbDto) // TODO not used
    {
        ArgumentNullException.ThrowIfNull(climbDto, nameof(climbDto));
        
        var climb = climbDto.ToEntity();

        var existingClimb = await GetClimbByIdAsync(climb.Id);

        if (existingClimb is null)
        {
            _logger.LogError("Existing climb is null");
            throw new InvalidOperationException(
                $"Climb with {climb.Id} does not exist and cannot be updated");
        }

        var recordsUpdated = await _climbRepository.UpdateAsync(existingClimb, climb);

        if (recordsUpdated != 1)
        {
            _logger.LogError($"{recordsUpdated} number of records updated, expected 1");
            throw new InvalidOperationException(
                $"Unexpected number of records updated. Expected 1 but got {recordsUpdated}");   
        }
    }

    private async Task DeleteClimbAsync(int id) // TODO not used
    {
        if (id <= 0)
        {
            _logger.LogError($"ID {id} is invalid");
            throw new ArgumentException($"Error deleting climb, ID: {id} is invalid.");   
        }

        var climbToBeDeleted = await GetClimbByIdAsync(id);

        if (climbToBeDeleted is null)
        {
            _logger.LogError("Climb to be deleted is null");
            throw new InvalidOperationException(
                $"Climb with {id} does not exist and cannot be deleted");   
        }

        var recordsDeleted = await _climbRepository.DeleteAsync(climbToBeDeleted);

        if (recordsDeleted != 1)
        {
            _logger.LogError($"{recordsDeleted} number of records deleted, should be 1");
            throw new InvalidOperationException(
                $"Unexpected number of records deleted. Expected 1 but got {recordsDeleted}");   
        }
    }
}