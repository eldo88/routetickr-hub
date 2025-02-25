using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ClimbController : ControllerBase
{
    private readonly IClimbService _climbService;

    public ClimbController(IClimbService climbService)
    {
        _climbService = climbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _climbService.GetAllAsync();
        if (!result.Success) { return NotFound(new { Message = result.ErrorMessage }); }

        return Ok(result.Data);
    }
}