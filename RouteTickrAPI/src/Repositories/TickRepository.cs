using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Mappers;
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
        var recordAdded = await _context.SaveChangesAsync();
        if (recordAdded == 1) return true;
        return false;
    }

    public async Task<bool> UpdateAsync(Tick tick)
    {
        var existingTick = await _context.Ticks.FindAsync(tick.Id);
        if (existingTick is null)
        {
            return false;
        }
        _context.Entry(existingTick).CurrentValues.SetValues(tick);
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
}