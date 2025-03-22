namespace RouteTickrAPI.Services;

public interface ICrudOperationsAsync<T>
{
    Task<ServiceResult<IEnumerable<T>>> GetAllAsync();
    Task<ServiceResult<T>> GetByIdAsync(int id);
    Task<ServiceResult<T>> AddAsync(T dto);
    Task<ServiceResult<T>> UpdateAsync(T dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}