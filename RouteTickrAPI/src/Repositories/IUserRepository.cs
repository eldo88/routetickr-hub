using Microsoft.EntityFrameworkCore.Storage;
using RouteTickr.Entities;

namespace RouteTickrAPI.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> FindAllAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<bool> AnyUserAsync(string userName);
    Task<User?> GetUserByUsernameAsync(string userName);
    Task<bool> AddUserAsync(User user);
}