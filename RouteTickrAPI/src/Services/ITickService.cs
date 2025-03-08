using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public interface ITickService
{
    Task<ServiceResult<IEnumerable<TickDto>>> GetAllAsync();
    Task<ServiceResult<TickDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds);
    Task<ServiceResult<TickDto>> AddAsync(TickDto tickDto);
    Task<ServiceResult<TickDto>> UpdateAsync(TickDto tickDto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
    Task<ServiceResult<Tick>> SaveTickAsync(TickDto tick);
    Task<ServiceResult<Tick>> SaveTickAsync(Tick tick);
}