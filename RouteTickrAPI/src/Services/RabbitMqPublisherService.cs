using System.Text;
using RabbitMQ.Client;

namespace RouteTickrAPI.Services;

public class RabbitMqPublisherService : IRabbitMqPublisherService
{
    private const string QueueName = "scrape_queue";
    
    public async void PublishUrl(string url)
    {
        
        var  factory = new ConnectionFactory();
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(
            queue: QueueName, 
            durable: false, 
            exclusive: false, 
            autoDelete: false,
            arguments: null);
        
        var body = Encoding.UTF8.GetBytes(url);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,             
            routingKey: QueueName,
            body: body);

        Console.WriteLine($" [x] Sent '{url}'");
    }
}