using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Extensions;

public static class FormFileExtensions
{
    public static async Task<ImportFileDto> ToImportFileDto(this IFormFile? file)
    {
        if (file is null || file.Length == 0) 
            throw new ArgumentException("File is empty or missing.");
        
        using var reader = new StreamReader(file.OpenReadStream());
        var content = await reader.ReadToEndAsync();
        
        return new ImportFileDto
        {
            FileName = file.FileName,
            Content = content
        };
    }
}