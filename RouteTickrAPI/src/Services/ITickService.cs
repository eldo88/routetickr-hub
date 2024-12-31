using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<ServiceResult<IEnumerable<Tick>>> GetAllAsync();
    Task<ServiceResult<Tick>> GetByIdAsync(int id);
    Task<ServiceResult<Tick>> AddAsync(Tick tick);
    Task<ServiceResult<Tick>> UpdateAsync(Tick tick);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<bool>> ImportFileAsync(IFormFile file);
}