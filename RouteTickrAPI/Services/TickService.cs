using System.Globalization;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using RouteTickrAPI.CsvMapper;

namespace RouteTickrAPI.Services;

public class TickService : ITickService
{
    private readonly ITickRepository _tickRepository;

    public TickService(ITickRepository tickRepository)
    {
        _tickRepository = tickRepository;
    }

    public async Task<IEnumerable<Tick>> GetAllAsync()
    {
        return await _tickRepository.GetAllAsync();
    }

    public async Task<Tick> GetByIdAsync(int id)
    {
        return await _tickRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Tick tick)
    {
        await _tickRepository.AddAsync(tick);
    }

    public async Task UpdateAsync(Tick tick)
    {
        await _tickRepository.UpdateAsync(tick);
    }

    public async Task DeleteAsync(int id)
    {
        await _tickRepository.DeleteAsync(id);
    }

    public async Task ImportFileAsync(IFormFile file)
    {
        using var stream = new StreamReader(file.OpenReadStream()) ;
        using var csvFile = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));

        csvFile.Context.RegisterClassMap<TickCsvImportMapper>();
        var dataFromFile = csvFile.GetRecords<Tick>().ToList();

        foreach (var tick in dataFromFile)
        {
            await _tickRepository.AddAsync(tick);
        }
    }
    
}