using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ImportFileController : ControllerBase
{
    private readonly IImportFileService _importFileService;

    public ImportFileController(IImportFileService importFileService)
    {
        _importFileService = importFileService;
    }
    
    [HttpPost]
    public async Task<IActionResult> ImportFile(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized("Could not identify the user.");
        }
            
        var fileDto = await file.ToImportFileDto();
        var result = await _importFileService.ProcessFile(fileDto, userId);
        if (result <= 0) 
            return BadRequest(new { Message = "File was either empty or error occurred."});

        return Ok($"File {fileDto.FileName} uploaded successfully, {result} records saved.");
    }
}