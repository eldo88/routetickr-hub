using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public interface IClimbService : ICrudOperations<ClimbDto>
{
    Task<Climb> GetOrSaveClimb(Climb climb);
}