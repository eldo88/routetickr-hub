using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public interface IClimbService
{
    Task<ServiceResult<IEnumerable<ClimbDto>>> GetAllAsync();
    Task<ServiceResult<Climb>> AddClimbIfNotExists(Climb climb);
    Task<ServiceResult<Climb>> GetByIdAsync(int id);
    Task<ServiceResult<Climb>> AddAsync(Climb climb);
}