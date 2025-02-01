using Microsoft.EntityFrameworkCore.Storage;

namespace RouteTickrAPI.Repositories;

public interface IUserRepository
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<bool> AnyUserAsync(string userName);
}