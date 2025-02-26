using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public interface IClimbRepository
{
    Task<int> AddClimb(Climb climb);
    Task<Climb?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Climb tick);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Climb>> GetAllAsync();
    Task<Climb?> GetByNameAndLocationAsync(string name, string location);
}