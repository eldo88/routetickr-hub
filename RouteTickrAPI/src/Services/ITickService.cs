using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync();
    Task<ServiceResult<TickDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds);
    Task<ServiceResult<TickDto>> AddAsync(TickDto tickDto);
    Task<ServiceResult<TickDto>> UpdateAsync(TickDto tickDto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<bool>> ImportFileAsync(IFormFile file);
}