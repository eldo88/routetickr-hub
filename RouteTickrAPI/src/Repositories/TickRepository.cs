using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public class TickRepository(ApplicationDbContext context) : ITickRepository
{
    public async Task<IEnumerable<Tick>> GetAllAsync()
    {
        return await context.Ticks
            .Include(t => t.Climb)
            .ToListAsync();
    }

    public async Task<Tick?> GetByIdAsync(int id)
    {
        return await context.Ticks
            .Include(t => t.Climb)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> AddAsync(Tick tick)
    {
        context.Ticks.Add(tick);
        var recordAdded = await context.SaveChangesAsync();
        return recordAdded == 2;
    }

    public async Task<bool> UpdateAsync(Tick tick)
    {
        var existingTick = await context.Ticks
            .Include(t => t.Climb)
            .FirstOrDefaultAsync(t => t.Id == tick.Id);
        
        if (existingTick is null)
        {
            return false;
        }
        context.Entry(existingTick).CurrentValues.SetValues(tick);
        var recordsUpdated = await context.SaveChangesAsync();
        return recordsUpdated == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tick = await context.Ticks
            .Include(t => t.Climb)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (tick is null)
        {
            return false;
        }
        context.Ticks.Remove(tick);
        var recordsDeleted = await context.SaveChangesAsync();
        return recordsDeleted == 1;
    }
    
    public async Task<int> GetTotalCountAsync()
    {
        return await context.Ticks.CountAsync();
    }

    public async Task<int?> GetPitchesAsync()
    {
        return await context.Ticks
            .Where(t => t.Pitches != null)
            .SumAsync(t => t.Pitches);
    }

    public async Task<List<string>> GetLocationAsync()
    {
        return await context.Ticks
            .Select(t => t.Location)
            .ToListAsync();
    }

    public async Task<List<string>> GetRatingAsync()
    {
        return await context.Ticks
            .Select(t => t.Rating)
            .ToListAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }
}