using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public class LocationTreeService : ILocationTreeService
{

    private readonly ApplicationDbContext _context;

    public LocationTreeService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddLocationAsync(string locationPath)
    {
        var paths = locationPath.Split('>', StringSplitOptions.TrimEntries);
        LocationNode? currentNode = null;

        foreach (var path in paths)
        {
            currentNode = await _context.Locations
                .Where(l => l.Name == path && (currentNode == null || l.ParentId == currentNode.Id))
                .FirstOrDefaultAsync();

            if (currentNode is not null) continue;
            currentNode = new LocationNode(path)
            {
                ParentId = currentNode?.Id 
            };

            _context.Locations.Add(currentNode);
            await _context.SaveChangesAsync();
        }
    }
}