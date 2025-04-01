using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TickController : ControllerBase
{
    private readonly ITickService _tickService;
    private readonly ITickRepository _tickRepository;

    public TickController(ITickService tickService, ITickRepository tickRepository)
    {
        _tickService = tickService;
        _tickRepository = tickRepository;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TickDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTicks()
    {
        var result = await _tickService.GetAllAsync();
        var tickDtos = result as TickDto[] ?? result.ToArray();
        if (tickDtos.Length == 0) 
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(tickDtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TickDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTick(int id)
    {
        if (id <= 0)
        {
            return BadRequest($"Invalid ID: {id}");
        }
        var result = await _tickService.GetByIdAsync(id);
        if (result is null) 
            return NotFound(new { Message = $"No tick found for id {id}"});
        
        return Ok(result);
    }

    [HttpGet]
    [Route("/ids")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByListOfIds([FromQuery] List<int> tickIds)
    {
        var result = await _tickService.GetByListOfIdsAsync(tickIds);
        if (result.Count == 0)
            return NotFound(new { Message = "No ticks found" });
        
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TickDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostTick(TickDto tickDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized("Could not identify the user.");
        }

        IDbContextTransaction transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            tickDto.UserId = userId;
            var result = await _tickService.AddAsync(tickDto, transaction);

            await _tickRepository.CommitTransactionAsync(transaction);
        
            return CreatedAtAction(nameof(GetTick), new { id = result.Id }, result);
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                await _tickRepository.RollbackTransactionAsync(transaction);
            }
            throw;
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TickDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutTick(int id, TickDto tickDto)
    {
        if (id != tickDto.Id || id < 0)
        {
            return BadRequest();
        }

        IDbContextTransaction transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            var result = await _tickService.UpdateAsync(tickDto, transaction);

            await _tickRepository.CommitTransactionAsync(transaction);
        
            return Ok(result);
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                await _tickRepository.RollbackTransactionAsync(transaction);
            }
            throw;
        }
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTick(int id)
    {
        if (id < 0)
            return BadRequest("Invalid Id");

        IDbContextTransaction transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            await _tickService.DeleteAsync(id, transaction);

            await _tickRepository.CommitTransactionAsync(transaction);
        
            return NoContent();
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                await _tickRepository.RollbackTransactionAsync(transaction);
            }
            throw;
        }
    }
}