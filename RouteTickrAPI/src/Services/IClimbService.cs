using RouteTickrAPI.DTOs;
using RouteTickr.Entities;

namespace RouteTickrAPI.Services;

public interface IClimbService : ICrudOperations<ClimbDto>
{
    Task<Climb> GetOrSaveClimb(Climb climb);
}