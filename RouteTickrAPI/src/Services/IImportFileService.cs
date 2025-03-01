using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IImportFileService
{
    Task<ServiceResult<bool>> ImportFileAsync(ImportFileDto fileDto);
}