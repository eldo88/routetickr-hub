using Microsoft.AspNetCore.Mvc;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Services;

namespace RouteTickrAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ImportFileController(IImportFileService importFileService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ImportFile(IFormFile file)
    {
        try
        {
            var fileDto = await file.ToImportFileDto();
            var result = await importFileService.ImportFileAsync(fileDto);
            if (!result.Success) { return BadRequest(new { Message = result.ErrorMessage }); }

            return Ok($"File {fileDto.FileName} uploaded successfully.");
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { e.Message });
        }
    }
}