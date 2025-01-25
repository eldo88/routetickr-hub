using RouteTickrAPI.DTOs;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public class ClimbingStatsService : IClimbingStatsService
{
    private readonly ITickRepository _tickRepository;

    public ClimbingStatsService(ITickRepository tickRepository)
    {
        _tickRepository = tickRepository;
    }


    private async Task<int> CalcTickTotal()
    {
        var totalTicks = await _tickRepository.GetTotalCountAsync();

        return totalTicks;
    }

    private async Task<int?> CalcTotalPitches()
    {
        var totalPitches = await _tickRepository.GetPitchesAsync();

        return totalPitches ?? 0;
    }

    private async Task<Dictionary<string, int>> CalcLocationVisits()
    {
        var locationVisits = new Dictionary<string, int>();

        var locationList = await _tickRepository.GetLocationAsync();
        var allLocations = new List<string>();

        try
        {
            foreach (var splitLocations in locationList.Select(location => location.Split('>', StringSplitOptions.TrimEntries)))
            {
                allLocations.AddRange(splitLocations);
            }

            foreach (var location in allLocations.Where(location => !locationVisits.TryAdd(location, 1)))
            {
                locationVisits[location]++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return locationVisits;
    }

    public async Task<ServiceResult<ClimbingStatsDto>> GetClimbingStats()
    {
        var totalTicks = await CalcTickTotal();
        var totalPitches = await CalcTotalPitches();
        var locationVisits = await CalcLocationVisits();

        var climbingStatsDto = new ClimbingStatsDto()
        {
            TotalTicks = totalTicks,
            TotalPitches = totalPitches,
            LocationVisits = locationVisits
        };
        
        return ServiceResult<ClimbingStatsDto>.SuccessResult(climbingStatsDto);
    }
}