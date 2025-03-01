using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ClimbController(IClimbService climbService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await climbService.GetAllAsync();
        if (!result.Success) { return NotFound(new { Message = result.ErrorMessage }); }

        return Ok(result.Data);
    }
}