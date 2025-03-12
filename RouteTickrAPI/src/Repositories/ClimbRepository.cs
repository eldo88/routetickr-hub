using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public class ClimbRepository : IClimbRepository
{
    private readonly ApplicationDbContext _context;

    public ClimbRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> AddClimb(Climb climb)
    {
        _context.Climbs.Add(climb);
        return await _context.SaveChangesAsync();
    }

    public async Task<Climb?> GetByIdAsync(int id)
    {
        return await _context.Climbs.FindAsync(id);
    }

    public async Task<int> UpdateAsync(Climb existingClimb, Climb tobeUpdated)
    {
        _context.Entry(existingClimb).CurrentValues.SetValues(tobeUpdated);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Climb climb)
    {
        _context.Climbs.Remove(climb);
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Climb>> GetAllAsync()
    {
        return await _context.Climbs.ToListAsync();
    }

    public async Task<Climb?> GetByNameAndLocationAsync(string name, string location)
    {
         return await _context.Climbs
            .Where(c => c.Name == name && c.Location == location)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}