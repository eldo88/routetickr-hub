using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TickController : ControllerBase
{
    private readonly ITickService _tickService;

    public TickController(ITickService tickService)
    {
        _tickService = tickService;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll()
    {
        var ticks = await _tickService.GetAllAsync();
        return Ok(ticks);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tick = await _tickService.GetByIdAsync(id);
        return Ok(tick);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Tick tick)
    {
        await _tickService.AddAsync(tick);
        return CreatedAtAction(nameof(GetAll), new { id = tick.Id }, tick);
    }

    [HttpPost]
    [Route("ImportFile")]
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
}