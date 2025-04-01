using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ImportFileController : ControllerBase
{
    private readonly IImportFileService _importFileService;
    private readonly ITickRepository _tickRepository;

    public ImportFileController(IImportFileService importFileService, ITickRepository tickRepository)
    {
        _importFileService = importFileService;
        _tickRepository = tickRepository;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFile(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized("Could not identify the user.");
        }

        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _tickRepository.BeginTransactionAsync();
            var fileDto = await file.ToImportFileDto();
            var result = await _importFileService.ProcessFile(fileDto, userId, transaction);
            if (result <= 0) 
                return BadRequest(new { Message = "File was either empty or error occurred."});

            await _tickRepository.CommitTransactionAsync(transaction);

            return Ok($"File {fileDto.FileName} uploaded successfully, {result} records saved.");
        }
        catch (Exception e)
        {
            if (transaction != null)
            {
                await _tickRepository.RollbackTransactionAsync(transaction);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Failed to import file. See logs for details." });
        }
        
    }
}