using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.DTOs;
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
        var climbDtos = result as ClimbDto[] ?? result.ToArray();
        if (climbDtos.Length == 0) { return NotFound(new { Message = "No climbs found." }); }

        return Ok(climbDtos);
    }
}