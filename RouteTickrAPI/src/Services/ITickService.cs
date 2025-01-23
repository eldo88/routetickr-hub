using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync();
    Task<ServiceResult<TickDto>> GetByIdAsync(int id);
    Task<ServiceResult<Tick>> AddAsync(Tick tick);
    Task<ServiceResult<TickDto>> UpdateAsync(TickDto tickDto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<bool>> ImportFileAsync(IFormFile file);
}