using RouteTickrAPI.Data;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public class LocationService : ILocationService
{
    private readonly ILocationTreeService _locationService;
    private readonly ApplicationDbContext _context;

    public LocationService(ILocationTreeService locationTreeService, ApplicationDbContext context)
    {
        _locationService = locationTreeService;
        _context = context;
    }
    
    public async Task AddLocationsAsync(string locations)
    {
        await _locationService.AddLocationAsync(locations);
    }

    public async Task<List<LocationNode>> GetALl()
    {
        return _context.Locations.ToList();
    }
}