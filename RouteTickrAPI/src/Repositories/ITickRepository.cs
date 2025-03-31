using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickr.Entities;

namespace RouteTickrAPI.Repositories;

public interface ITickRepository
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick?> GetByIdAsync(int id);
    Task<int> AddAsync(Tick tick);
    Task<int> UpdateAsync(Tick existingTick, Tick updateTo);
    Task<int> DeleteAsync(Tick tick);
    Task<int> GetTotalCountAsync();
    Task<int?> GetPitchesAsync();
    Task<List<string>> GetLocationAsync();
    Task<List<string>> GetRatingAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
}