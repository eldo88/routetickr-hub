using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Models;
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
        if (!result.Success)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }
        return Ok(result.Data);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _tickService.GetByIdAsync(id);
        if (!result.Success)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Tick tick)
    {
        await _tickService.AddAsync(tick);
        return CreatedAtAction(nameof(GetAll), new { id = tick.Id }, tick);
    }

    [HttpPost]
    public async Task<IActionResult> ImportFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("File does not contain data");
        }

        try
        {
            await _tickService.ImportFileAsync(file);
            return Ok($"Ticks uploaded from file: {file.Name}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, "Error saving file");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(Tick tick)
    {
        await _tickService.UpdateAsync(tick);
        return CreatedAtAction(nameof(GetAll), new { id = tick.Id }, tick);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _tickService.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound($"Tick with ID: {id} was not found");
        }

        return NoContent();
    }
}