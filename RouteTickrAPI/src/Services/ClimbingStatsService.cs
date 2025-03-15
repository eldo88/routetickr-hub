using Microsoft.Extensions.Caching.Memory;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;


namespace RouteTickrAPI.Services;

public class ClimbingStatsService : IClimbingStatsService
{
    private readonly ITickRepository _tickRepository;
    private readonly IMemoryCache _cache;
    // TODO implement error handling in this class
    public ClimbingStatsService(ITickRepository tickRepository, IMemoryCache cache)
    {
        _tickRepository = tickRepository;
        _cache = cache;
    }
    
    public async Task<ServiceResult<List<int>>> GetTickIdsByState(string state)
    {
        try
        {
            if (_cache.TryGetValue($"TickIds_{state}", out List<int>? cachedTickIds))
            {
                return cachedTickIds is not null
                    ? ServiceResult<List<int>>.SuccessResult(cachedTickIds)
                    : ServiceResult<List<int>>.ErrorResult($"No tick IDs cached for state: {state}");
            }
            
            var locationsWithTickIds = await GetLocationWithTickIds();
            
            var tickIds = locationsWithTickIds
                .Where(locationEntry => locationEntry.Key.Contains(state, StringComparison.OrdinalIgnoreCase))
                .SelectMany(locationEntry => locationEntry.Value)
                .ToList();

            if (tickIds.Count == 0)
            {
                return ServiceResult<List<int>>.NotFoundResult($"No tick IDs found for state: {state}");
            }
            
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            _cache.Set($"TickIds_{state}", tickIds, cacheOptions);

            return ServiceResult<List<int>>.SuccessResult(tickIds);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResult<ClimbingStatsDto>> GetClimbingStats()
    {
        try
        {
            var stats = await _cache.GetOrCreateAsync("ClimbingStats", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);

                return new ClimbingStatsDto
                {
                    TotalTicks = await CalcTickTotal(),
                    TotalPitches = await CalcTotalPitches(),
                    LocationVisits = await CalcVisitsPerLocation(),
                    TicksPerGrade = await CalcTicksPerGrade()
                };
            });

            return stats is not null
                ? ServiceResult<ClimbingStatsDto>.SuccessResult(stats)
                : ServiceResult<ClimbingStatsDto>.NotFoundResult("Failed to retrieve climbing stats.");
        }
        catch (ArgumentNullException e)
        {
            return ServiceResult<ClimbingStatsDto>.ErrorResult("Unexpected null value encountered calculating stats.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private async Task<int> CalcTickTotal()
    {
        var totalTicks = await _tickRepository.GetTotalCountAsync();

        return totalTicks;
    }

    private async Task<int> CalcTotalPitches()
    {
        var totalPitches = await _tickRepository.GetPitchesAsync();

        return totalPitches ?? 0;
    }

    private async Task<Dictionary<string, int>> CalcVisitsPerLocation()
    {
        var locationVisits = new Dictionary<string, int>();
        var allLocations = new List<string>();
        var locationList = await _tickRepository.GetLocationAsync();
            
        foreach (var splitLocations in locationList.Select(locations => locations.Split('>', StringSplitOptions.TrimEntries)))
        {
            allLocations.AddRange(splitLocations);
        }
        
        foreach (var location in allLocations)
        {
            locationVisits.IncrementCount(location);
        }
            
        return locationVisits;
    }

    private async Task<Dictionary<string, List<int>>> GetLocationWithTickIds()
    {
        var ticks = await _tickRepository.GetAllAsync();
        var locationWithTickIds = new Dictionary<string, List<int>>();
        
        foreach (var tick in ticks)
        {
            if (string.IsNullOrWhiteSpace(tick.Location)) continue;
            var locations = tick.Location.Split('>', StringSplitOptions.TrimEntries);

            foreach (var location in locations)
            {
                locationWithTickIds.AddToCollection(location, tick.Id);
            }
        }

        return locationWithTickIds;
    }

    private async Task<Dictionary<string, int>> CalcTicksPerGrade()
    {
        var gradeCounts = new Dictionary<string, int>();
        var grades = await _tickRepository.GetRatingAsync();
        
        foreach (var grade in grades)
        {
            gradeCounts.IncrementCount(grade);
        }
            
        return gradeCounts;
    }
}