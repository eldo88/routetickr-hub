using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<IEnumerable<Tick>> GetAllAsync();
    Task<Tick> GetByIdAsync(int id);
    Task AddAsync(Tick tick);
    Task UpdateAsync(Tick tick);
    Task DeleteAsync(int id);
    Task ImportFileAsync(IFormFile file);
}