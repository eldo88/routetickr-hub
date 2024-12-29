using RouteTickrAPI.Models;

namespace RouteTickrAPI.Repositories;

public interface ITickRepository
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick?> GetByIdAsync(int id);
    Task AddAsync(Tick tick);
    Task UpdateAsync(Tick tick);
    Task<bool> DeleteAsync(int id);
}