using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface ITickService : ICrudOperations<TickDto>
{ 
    Task<List<TickDto>> GetByListOfIdsAsync(List<int> tickIds);
}