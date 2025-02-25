using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IClimbService
{
    Task<ServiceResult<IEnumerable<ClimbDto>>> GetAllAsync();
}