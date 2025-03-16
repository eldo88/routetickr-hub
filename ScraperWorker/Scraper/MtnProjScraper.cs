namespace ScraperWorker.Scraper;

public class MtnProjScraper
{
    private readonly Queue<string> _urlQueue;

    public MtnProjScraper()
    {
        _urlQueue = new Queue<string>();
    }
    public void EnqueueUrls(string url)
    {
        _urlQueue.Enqueue(url);
    }

    public bool HasUrls()
    {
        return _urlQueue.Count > 0;
    }
    
    private static async Task<string> GetHtmlAsync(string url)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; HttpClient/1.0)");
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task Scrape()
    {
        var url = _urlQueue.Dequeue();
        var html = await GetHtmlAsync(url);
        Console.WriteLine(html);
    }
}