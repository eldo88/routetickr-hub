using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
using RouteTickrAPI.Models;
using InvalidOperationException = System.InvalidOperationException;

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

    public async Task AddAsync(Tick tick)
    {
        _context.Ticks.Add(tick);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tick tick)
    {
        _context.Ticks.Update(tick);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tick = await _context.Ticks.FindAsync(id);

        if (tick is null)
        {
            return false;
        }
        _context.Ticks.Remove(tick);
        await _context.SaveChangesAsync();
        return true;
    }
}