namespace RouteTickrAPI.Services;

public interface IClimbingStatsService
{
    Task<int> CalcTickTotal();
    Task<int?> CalcTotalPitches();
    Task<Dictionary<string, int>> CalcLocationVisits();
}