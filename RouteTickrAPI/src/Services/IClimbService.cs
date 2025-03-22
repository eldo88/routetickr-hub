using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public interface IClimbService : ICrudOperationsAsync<ClimbDto>
{
    Task<Climb> GetOrSaveClimb(Climb climb);
}