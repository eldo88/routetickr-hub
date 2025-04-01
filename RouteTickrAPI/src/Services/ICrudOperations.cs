using Microsoft.EntityFrameworkCore.Storage;

namespace RouteTickrAPI.Services;

public interface ICrudOperations<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T dto, IDbContextTransaction? transaction);
    Task<T> UpdateAsync(T dto, IDbContextTransaction? transaction);
    Task DeleteAsync(int id, IDbContextTransaction? transaction);
}