
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ClimbingStatsController(IClimbingStatsService climbingStatsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClimbingStats()
    {
        var result = await climbingStatsService.GetClimbingStats();
        if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetTickIdsPerState(string state)
    {
        var result = await climbingStatsService.GetTickIdsByState(state);
        return Ok(result);
    }
}