using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Services;

public interface ILocationService
{
    Task AddLocationsAsync(string locations);
    Task<List<LocationNode>> GetALl();
}