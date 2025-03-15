namespace RouteTickrAPI.Services;

public interface IRabbitMqPublisherService
{
    void PublishUrl(string url);
}