namespace RouteTickrAPI.Services;

public interface ILocationTreeService
{
    Task AddLocationAsync(string locationPath);
}