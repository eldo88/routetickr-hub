using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public class ClimbRepository(ApplicationDbContext context) : IClimbRepository
{
    public async Task<int> AddClimb(Climb climb)
    {
        context.Climbs.Add(climb);
        return await context.SaveChangesAsync();
    }

    public async Task<Climb?> GetByIdAsync(int id)
    {
        return await context.Climbs.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(Climb climb)
    {
        var existingClimb = await context.Climbs.FindAsync(climb.Id);
        if (existingClimb is null) return false;
        context.Entry(existingClimb).CurrentValues.SetValues(climb);
        var recordsUpdated = await context.SaveChangesAsync();
        return recordsUpdated == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var climb = await context.Climbs.FindAsync(id);
        if (climb is null) return false;
        context.Climbs.Remove(climb);
        var recordsDeleted = await context.SaveChangesAsync();
        return recordsDeleted == 1;
    }

    public async Task<IEnumerable<Climb>> GetAllAsync()
    {
        return await context.Climbs.ToListAsync();
    }

    public async Task<Climb?> GetByNameAndLocationAsync(string name, string location)
    {
         return await context.Climbs
            .Where(c => c.Name == name && c.Location == location)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }
}