using Microsoft.EntityFrameworkCore.Storage;

namespace RouteTickrAPI.Services;

public interface ICrudOperations<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T dto);
    Task<T> UpdateAsync(T dto);
    Task DeleteAsync(int id);
}