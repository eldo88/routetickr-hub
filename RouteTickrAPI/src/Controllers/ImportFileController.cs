using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
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
        try
        {
            var fileDto = await file.ToImportFileDto();
            var result = await _importFileService.ProcessFile(fileDto);
            if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }

            return Ok($"File {fileDto.FileName} uploaded successfully.");
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { e.Message });
        }
    }
}