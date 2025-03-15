namespace RouteTickrAPI.Services;

public interface IRabbitMqPublisher
{
    void PublishUrl(string url);
}