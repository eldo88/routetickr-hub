using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ScraperWorker.Scraper;

namespace ScraperWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly MtnProjScraper _scraper = new();
    private IConnection? _connection;
    private IChannel? _channel;

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
        try
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory();
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                _logger.LogInformation("Connected to RabbitMQ");

                await _channel.QueueDeclareAsync(
                    queue: "scrape_queue", 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null, 
                    cancellationToken: stoppingToken);

                _logger.LogInformation("Queue declared: scrape_queue");

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _scraper.EnqueueUrls(message);
                    _logger.LogInformation("Received message: {Message}", message);
                    return Task.CompletedTask;
                };

                await _channel.BasicConsumeAsync(
                    queue: "scrape_queue", 
                    autoAck: true, 
                    consumer: consumer, 
                    cancellationToken: stoppingToken);

                _logger.LogInformation("Started consuming messages from scrape_queue");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error listening for messages");
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
                _logger.LogError(e, "Error during scraping");
            }
            // Wait 60 seconds before next scrape to adhere to MtnProj TOS https://www.mountainproject.com/robots.txt
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping worker service...");

        if (_channel is not null)
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync(cancellationToken);
            await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
    
    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}