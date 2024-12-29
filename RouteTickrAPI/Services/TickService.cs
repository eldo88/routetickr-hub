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

    public async Task<ServiceResult<IEnumerable<Tick>>> GetAllAsync()
    {
        try
        {
            var ticks = await _tickRepository.GetAllAsync();
            var enumerable = ticks.ToList();
            return enumerable.Count == 0 ? ServiceResult<IEnumerable<Tick>>.ErrorResult("No ticks found.") 
                : ServiceResult<IEnumerable<Tick>>.SuccessResult(enumerable);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetAllAsync: {e.Message}");
            throw;
        }
    }

    public async Task<ServiceResult<Tick>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return ServiceResult<Tick>.ErrorResult("ID must be greater than zero.");
            }
            
            var tick = await _tickRepository.GetByIdAsync(id);
            if (tick is null)
            {
                return ServiceResult<Tick>.ErrorResult($"Tick with ID: {id} not found.");
            }
            return ServiceResult<Tick>.SuccessResult(tick);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetByIdAsync: {e.Message}");
            throw;
        }
    }

    public async Task AddAsync(Tick tick)
    {
        await _tickRepository.AddAsync(tick);
    }

    public async Task UpdateAsync(Tick tick)
    {
        await _tickRepository.UpdateAsync(tick);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _tickRepository.DeleteAsync(id);
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