using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IClimbingStatsService
{
    Task<ServiceResult<ClimbingStatsDto>> GetClimbingStats();
}