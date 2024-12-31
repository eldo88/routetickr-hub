using RouteTickrAPI.Models;

namespace RouteTickrAPI.Repositories;

public interface ITickRepository
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick?> GetByIdAsync(int id);
    Task<bool> AddAsync(Tick tick);
    Task<bool> UpdateAsync(Tick tick);
    Task<bool> DeleteAsync(int id);
}