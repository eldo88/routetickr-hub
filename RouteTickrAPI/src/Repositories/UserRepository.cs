using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<IEnumerable<User>> FindAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> AnyUserAsync(string userName)
    {
        return await _context.Users.AnyAsync(u => u.Username == userName);
    }

    public async Task<User?> GetUserByUsernameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
    }

    public async Task<bool> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        var recordAdded = await _context.SaveChangesAsync();
        return recordAdded == 1;
    }
}