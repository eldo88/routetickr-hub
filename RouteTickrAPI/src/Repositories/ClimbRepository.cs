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

    public async Task<bool> UpdateAsync(Climb climb)
    {
        var existingClimb = await _context.Climbs.FindAsync(climb.Id);
        if (existingClimb is null) return false;
        _context.Entry(existingClimb).CurrentValues.SetValues(climb);
        var recordsUpdated = await _context.SaveChangesAsync();
        return recordsUpdated == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var climb = await _context.Climbs.FindAsync(id);
        if (climb is null) return false;
        _context.Climbs.Remove(climb);
        var recordsDeleted = await _context.SaveChangesAsync();
        return recordsDeleted == 1;
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