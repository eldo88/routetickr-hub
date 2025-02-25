
using RouteTickrAPI.Repositories;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Extensions;

namespace RouteTickrAPI.Services;

public class TickService : ITickService
{
    private readonly ITickRepository _tickRepository;
    private readonly IClimbService _climbService;

    public TickService(ITickRepository tickRepository,IClimbService climbService)
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
            if (id <= 0) 
                return ServiceResult<TickDto>.ErrorResult("ID must be greater than zero.");
            
            var tick = await _tickRepository.GetByIdAsync(id);
            if (tick is null) 
                return ServiceResult<TickDto>.ErrorResult($"Tick with ID: {id} not found.");
            
            var tickDto = tick.ToTickDto();
            
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
            if (tickIds.Count == 0) 
                return ServiceResult<List<TickDto>>.ErrorResult("ID list is empty");
            
            var tickDtos = new List<TickDto>();
            
            foreach (var id in tickIds)
            {
                var tick = await _tickRepository.GetByIdAsync(id);
                if (tick is null) continue;
                var tickDto = tick.ToTickDto();
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
            if (tickDto.Climb is null) return ServiceResult<TickDto>.ErrorResult("Climb is null");
            var result = await _climbService.AddClimbIfNotExists(tickDto.Climb);
            if (result.Data is null) return ServiceResult<TickDto>.ErrorResult("Climb is null.");
            
            var tick = tickDto.ToTickEntity(result.Data);
            var isTickAdded = await _tickRepository.AddAsync(tick);
            if (!isTickAdded) return ServiceResult<TickDto>.ErrorResult("Error adding tick.");
            
            var tickAdded = tick.ToTickDto();
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
            var recordToBeUpdated = await _tickRepository.GetByIdAsync(tickDto.Id);
            if (recordToBeUpdated is null) 
                return ServiceResult<TickDto>.ErrorResult($"Tick with ID: {tickDto.Id} does not exist");
            
            var tick = tickDto.ToEntity();
            var isUpdated = await _tickRepository.UpdateAsync(tick);
            
            return isUpdated 
                ? ServiceResult<TickDto>.SuccessResult(tickDto)
                : ServiceResult<TickDto>.ErrorResult($"Error updating tick with ID: {tickDto.Id}");
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
    
}