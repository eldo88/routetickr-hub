using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public interface ITickRepository
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick?> GetByIdAsync(int id);
    Task<bool> AddAsync(Tick tick);
    Task<bool> UpdateAsync(Tick tick);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<int?> GetPitchesAsync();
    Task<List<string>> GetLocationAsync();
    Task<List<string>> GetRatingAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}