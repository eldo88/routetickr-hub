using Microsoft.EntityFrameworkCore.Storage;
using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IImportFileService
{
    Task<int> ProcessFile(ImportFileDto fileDto, string userId, IDbContextTransaction? transaction);
    Task<int> SaveFileContentsAsync(List<TickDto> dataFromFile, string userId, IDbContextTransaction? transaction);
}