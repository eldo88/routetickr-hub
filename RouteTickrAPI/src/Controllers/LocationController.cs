using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Entities;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class LocationController(ILocationService locationService) : ControllerBase
{
    [HttpGet]
    public async Task<List<LocationNode>> GetLocations()
    {
        var result = await locationService.GetALl();

        return result;
    }
}