using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Repositories;

public class TickRepository : ITickRepository
{

    private readonly ApplicationDbContext _context;

    public TickRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Tick>> GetAllAsync()
    {
        return await _context.Ticks
            .Include(t => t.Climb)
            .ToListAsync();
    }

    public async Task<Tick?> GetByIdAsync(int id)
    {
        return await _context.Ticks
            .Include(t => t.Climb)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> AddAsync(Tick tick)
    {
        _context.Ticks.Add(tick);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Tick existingTick, Tick tickToUpdate)
    {
        _context.Entry(existingTick).CurrentValues.SetValues(tickToUpdate);
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id) //create service method
    {
        var tick = await _context.Ticks
            .Include(t => t.Climb)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (tick is null)
        {
            return false;
        }
        _context.Ticks.Remove(tick);
        var recordsDeleted = await _context.SaveChangesAsync();
        return recordsDeleted == 1;
    }
    
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Ticks.CountAsync();
    }

    public async Task<int?> GetPitchesAsync()
    {
        return await _context.Ticks
            .Where(t => t.Pitches != null)
            .SumAsync(t => t.Pitches);
    }

    public async Task<List<string>> GetLocationAsync()
    {
        return await _context.Ticks
            .Select(t => t.Location)
            .ToListAsync();
    }

    public async Task<List<string>> GetRatingAsync()
    {
        return await _context.Ticks
            .Select(t => t.Rating)
            .ToListAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}