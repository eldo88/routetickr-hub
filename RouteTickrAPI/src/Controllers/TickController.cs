using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TickController : ControllerBase
{
    private readonly ITickService _tickService;

    public TickController(ITickService tickService)
    {
        _tickService = tickService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTicks()
    {
        var result = await _tickService.GetAllAsync();
        var tickDtos = result as TickDto[] ?? result.ToArray();
        if (tickDtos.Length == 0) 
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(tickDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTick(int id)
    {
        var result = await _tickService.GetByIdAsync(id);
        if (result is null) 
            return NotFound(new { Message = $"No tick found for id {id}"});
        
        return Ok(result);
    }

    [HttpGet]
    [Route("/ids")]
    public async Task<IActionResult> GetByListOfIds([FromQuery] List<int> tickIds)
    {
        var result = await _tickService.GetByListOfIdsAsync(tickIds);
        if (result.Count == 0)
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostTick(TickDto tickDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized("Could not identify the user.");
        }
        
        tickDto.UserId = userId;
        var result = await _tickService.AddAsync(tickDto);
        
        return CreatedAtAction(nameof(GetTick), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutTick(int id, TickDto tickDto)
    {
        if (id != tickDto.Id)
        {
            return BadRequest();
        }
        
        var result = await _tickService.UpdateAsync(tickDto);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTick(int id)
    {
        await _tickService.DeleteAsync(id);
        
        return NoContent();
    }
}