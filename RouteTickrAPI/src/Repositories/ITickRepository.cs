using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public interface ITickRepository
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick?> GetByIdAsync(int id);
    Task<int> AddAsync(Tick tick);
    Task<int> UpdateAsync(Tick existingTick, Tick tickToUpdate);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<int?> GetPitchesAsync();
    Task<List<string>> GetLocationAsync();
    Task<List<string>> GetRatingAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}