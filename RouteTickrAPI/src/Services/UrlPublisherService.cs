using System.Text;
using RabbitMQ.Client;

namespace RouteTickrAPI.Services;

public class UrlPublisherService : IPublisherService
{
    private const string QueueName = "scrape_queue";
    
    public async void PublishMessage(string url)
    {
        try
        {
            var factory = new ConnectionFactory();
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
        catch (Exception e)
        { //Swallow exception since this is for a background service
            Console.WriteLine($"Error processing URL for scraping {e.Message}");
        }
    }
}