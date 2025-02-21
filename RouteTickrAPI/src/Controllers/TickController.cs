using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;
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
        if (!result.Success) { return NotFound(new { Message = result.ErrorMessage }); }
        return Ok(result.Data);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _tickService.GetByIdAsync(id);
        if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetByListOfIds([FromQuery] List<int> tickIds)
    {
        var result = await _tickService.GetByListOfIdsAsync(tickIds);
        if (!result.Success) return BadRequest(new { Message = result.ErrorMessage });
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TickDto tickDto)
    {
        var tickAdded = await _tickService.AddAsync(tickDto);
        if (!tickAdded.Success) { return BadRequest(new { Message = tickAdded.ErrorMessage }); }
        return CreatedAtAction(nameof(GetAll), new { id = tickAdded.Data.Id }, tickAdded.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TickDto tickDto)
    {
        if (!ModelState.IsValid) { return BadRequest(new { Message = "Invalid model state.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) }); }
        var result = await _tickService.UpdateAsync(tickDto);
        if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }
        return Ok(result.Data);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _tickService.DeleteAsync(id);
        if (!result.Success) { return NotFound(new { Message = result.ErrorMessage }); }
        return NoContent();
    }
}