using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
using RouteTickrAPI.Models;

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
        return await _context.Ticks.ToListAsync();
    }

    public async Task<Tick?> GetByIdAsync(int id)
    {
        return await _context.Ticks.FindAsync(id);
    }

    public async Task<bool> AddAsync(Tick tick)
    {
        _context.Ticks.Add(tick);
        var recordsAdded = await _context.SaveChangesAsync();
        return recordsAdded == 1;
    }

    public async Task<bool> UpdateAsync(Tick tick)
    {
        _context.Ticks.Update(tick);
        var recordsUpdated = await _context.SaveChangesAsync();
        return recordsUpdated == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tick = await _context.Ticks.FindAsync(id);
        
        if (tick is null)
        {
            return false;
        }
        _context.Ticks.Remove(tick);
        var recordsDeleted = await _context.SaveChangesAsync();
        return recordsDeleted == 1;
    }
}