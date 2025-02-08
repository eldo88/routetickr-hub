using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
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
        if (file.Length == 0) { return BadRequest("File does not contain data"); }
        var result = await _importFileService.ImportFileAsync(file);
        if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }
        return NoContent();
    }
}