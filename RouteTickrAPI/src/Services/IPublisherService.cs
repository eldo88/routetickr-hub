namespace RouteTickrAPI.Services;

public interface IPublisherService : IAsyncDisposable
{
    Task InitializeAsync();
    Task PublishMessageAsync(string message);
}