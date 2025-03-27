using Microsoft.EntityFrameworkCore.Storage;
using RouteTickr.Entities;

namespace RouteTickrAPI.Repositories;

public interface IClimbRepository
{
    Task<int> AddClimb(Climb climb);
    Task<Climb?> GetByIdAsync(int id);
    Task<int> UpdateAsync(Climb existingClimb, Climb tobeUpdated);
    Task<int> DeleteAsync(Climb climb);
    Task<IEnumerable<Climb>> GetAllAsync();
    Task<Climb?> GetByNameAndLocationAsync(string name, string location);
    Task<IDbContextTransaction> BeginTransactionAsync();
}