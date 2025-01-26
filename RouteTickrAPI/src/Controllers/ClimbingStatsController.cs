
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ClimbingStatsController : ControllerBase
{
    private readonly IClimbingStatsService _climbingStatsService;

    public ClimbingStatsController(IClimbingStatsService climbingStatsService)
    {
        _climbingStatsService = climbingStatsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetClimbingStats()
    {
        var result = await _climbingStatsService.GetClimbingStats();
        if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetTickIdsPerState(string state)
    {
        var result = await _climbingStatsService.GetTickIdsByState(state);
        return Ok(result);
    }
}