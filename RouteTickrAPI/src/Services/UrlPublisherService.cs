using System.Text;
using RabbitMQ.Client;

namespace RouteTickrAPI.Services;

public class UrlPublisherService : IPublisherService
{
    private const string QueueName = "scrape_queue";
    private IConnection? _connection;
    private IChannel? _channel;
    
    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory();
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public async Task PublishMessageAsync(string url)
    {
        if (_channel is null)
            throw new InvalidOperationException("Publisher has not been initialized. Call InitializeAsync() first.");

        var body = Encoding.UTF8.GetBytes(url);
        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: QueueName,
            body: body);

        Console.WriteLine($" [x] Sent '{url}'");
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}