using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface ITickService : ICrudOperationsAsync<TickDto>
{ 
    Task<ServiceResult<List<TickDto>>> GetByListOfIdsAsync(List<int> tickIds);
    Task<TickDto> SaveTickAsync(TickDto tickDto);
}