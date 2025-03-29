using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.DTOs;
using RouteTickr.Entities;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TickController : ControllerBase
{
    private readonly ITickService _tickService;

    public TickController(ITickService tickService)
    {
        _tickService = tickService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tickService.GetAllAsync();
        var tickDtos = result as TickDto[] ?? result.ToArray();
        if (tickDtos.Length == 0) 
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(tickDtos);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _tickService.GetByIdAsync(id);
        if (result is null) 
            return NotFound(new { Message = $"No tick found for id {id}"});
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByListOfIds([FromQuery] List<int> tickIds)
    {
        var result = await _tickService.GetByListOfIdsAsync(tickIds);
        if (result.Count == 0)
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TickDto tickDto)
    {
        var result = await _tickService.AddAsync(tickDto);
        
        //TODO return link to newly added tick?
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TickDto tickDto)
    { 
        var result = await _tickService.UpdateAsync(tickDto);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tickService.DeleteAsync(id);
        
        return NoContent();
    }
}