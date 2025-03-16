using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IImportFileService
{
    Task<ServiceResult<int>> ProcessFile(ImportFileDto fileDto);
    Task<bool> SaveFileContentsAsync(List<TickDto> dataFromFile);
}