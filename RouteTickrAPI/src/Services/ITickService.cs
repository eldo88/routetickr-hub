using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<ServiceResult<IEnumerable<Tick>>> GetAllAsync();
    Task<ServiceResult<Tick>> GetByIdAsync(int id);
    Task AddAsync(Tick tick);
    Task UpdateAsync(Tick tick);
    Task<bool> DeleteAsync(int id);
    Task ImportFileAsync(IFormFile file);
}