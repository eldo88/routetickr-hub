using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IImportFileService
{
    Task<int> ProcessFile(ImportFileDto fileDto, string userId);
    Task<int> SaveFileContentsAsync(List<TickDto> dataFromFile, string userId);
}