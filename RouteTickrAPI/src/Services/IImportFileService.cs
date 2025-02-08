namespace RouteTickrAPI.Services;

public interface IImportFileService
{
    Task<ServiceResult<bool>> ImportFileAsync(IFormFile file);
}