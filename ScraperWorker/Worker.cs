using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ScraperWorker.Scraper;

namespace ScraperWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly MtnProjScraper _scraper = new();

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.WhenAll(
            ListenForMessages(stoppingToken), 
            ProcessScrapingQueue(stoppingToken)
        );
    }

    private async Task ListenForMessages(CancellationToken stoppingToken)
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
                    _scraper.EnqueueUrls(message);
                    _logger.LogInformation("Message from queue: {message}", DateTimeOffset.Now);
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

                //await Task.Delay(1000, stoppingToken);
            }
            catch (Exception e)
            {//Swallow exception since this is for a background service
                _logger.LogError("Error listening for messages {e.Message}", DateTimeOffset.Now);
            }
        }
    }
    
    private async Task ProcessScrapingQueue(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_scraper.HasUrls())
                {
                    await _scraper.Scrape();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error during scraping: {e.Message}", DateTimeOffset.Now);
            }
            // Wait 60 seconds before next scrape to adhere to MtnProj TOS https://www.mountainproject.com/robots.txt
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }
}