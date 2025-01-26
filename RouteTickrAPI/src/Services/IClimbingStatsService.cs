using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IClimbingStatsService
{
    Task<ServiceResult<ClimbingStatsDto>> GetClimbingStats();
    Task<ServiceResult<List<int>>> GetTickIdsByState(string state);
}