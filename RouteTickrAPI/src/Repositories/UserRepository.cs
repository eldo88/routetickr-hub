using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickr.Entities;
using RouteTickr.DbContext;

namespace RouteTickrAPI.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }

    public async Task<IEnumerable<User>> FindAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<bool> AnyUserAsync(string userName)
    {
        return await context.Users.AnyAsync(u => u.Username == userName);
    }

    public async Task<User?> GetUserByUsernameAsync(string userName)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == userName);
    }

    public async Task<bool> AddUserAsync(User user)
    {
        context.Users.Add(user);
        var recordAdded = await context.SaveChangesAsync();
        return recordAdded == 1;
    }
}