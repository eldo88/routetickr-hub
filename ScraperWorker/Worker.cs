using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ScraperWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory();
                await using var connection = await factory.CreateConnectionAsync(stoppingToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
            
                await channel.QueueDeclareAsync(
                    queue: "scrape_queue", 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null, 
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Message from queue: {message}", DateTimeOffset.Now);
                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync(
                    "scrape_queue", 
                    autoAck: true, 
                    consumer: consumer, 
                    cancellationToken: stoppingToken);
            
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception e)
            {//Swallow exception since this is for a background service
                Console.WriteLine($"Error in worker service: {e.Message}");
            }
        }
    }
}