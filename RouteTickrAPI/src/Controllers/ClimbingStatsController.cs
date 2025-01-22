
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ClimbingStatsController : ControllerBase
{
    private IClimbingStatsService _climbingStatsService;

    public ClimbingStatsController(IClimbingStatsService climbingStatsService)
    {
        _climbingStatsService = climbingStatsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocationVisits()
    {
        var result = await _climbingStatsService.CalcLocationVisits();

        if (result.Count == 0)
        {
            return BadRequest("Error");
        }

        return Ok(result);
    }
}