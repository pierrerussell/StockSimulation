using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockSimulation.Domain.StockPrices;

namespace StockSimulation.EfCore.StockPrices;

public class StockPriceRepository : IStockPriceRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<StockPriceRepository> _logger;

    public StockPriceRepository(ApplicationDbContext dbContext, ILogger<StockPriceRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    


    public IQueryable<StockPrice> GetQueryable()
    {
        return _dbContext.StockPrices;
    }

    public async Task<StockPrice> AddAsync(StockPrice entity)
    {
        await _dbContext.StockPrices.AddAsync(entity);
        return entity;
    }

    public StockPrice Update(StockPrice entity)
    {
        _dbContext.StockPrices.Update(entity);
        return entity;
    }

    public async Task<IEnumerable<StockPrice>> UpsertManyAsync(IEnumerable<StockPrice> entities)
    {
        var stockPricesList = entities.ToList();
        if (!stockPricesList.Any())
        {
            return stockPricesList;
        }

        // Get all unique symbols and date range from incoming entities
        var symbols = stockPricesList.Select(sp => sp.StockSymbol).Distinct().ToList();
        var minDate = stockPricesList.Min(sp => sp.Date);
        var maxDate = stockPricesList.Max(sp => sp.Date);

        // Get existing entities from the database matching these criteria
        var existingEntities = await _dbContext.StockPrices
            .Where(sp => symbols.Contains(sp.StockSymbol) && sp.Date >= minDate && sp.Date <= maxDate)
            .ToListAsync();

        // Create a dictionary of existing entities for quick lookup
        var existingDict = existingEntities.ToDictionary(
            sp => (sp.StockSymbol, sp.Date),
            sp => sp
        );

        var result = new List<StockPrice>();

        foreach (var newEntity in stockPricesList)
        {
            var key = (newEntity.StockSymbol, newEntity.Date);
            
            if (existingDict.TryGetValue(key, out var existingEntity))
            {
                // Update existing entity
                existingEntity.Open = newEntity.Open;
                existingEntity.Close = newEntity.Close;
                existingEntity.High = newEntity.High;
                existingEntity.Low = newEntity.Low;
                existingEntity.Volume = newEntity.Volume;
                existingEntity.Change = newEntity.Change;
                existingEntity.ChangePercent = newEntity.ChangePercent;
                existingEntity.Vwap = newEntity.Vwap;
                
                result.Add(existingEntity);
                _logger.LogDebug($"Updated stock price for {newEntity.StockSymbol} on {newEntity.Date}");
            }
            else
            {
                // Add new entity
                await _dbContext.StockPrices.AddAsync(newEntity);
                result.Add(newEntity);
                _logger.LogDebug($"Added new stock price for {newEntity.StockSymbol} on {newEntity.Date}");
            }
        }

        return result;
    }

    public void Delete(StockPrice entity)
    {
        
        _dbContext.StockPrices.Remove(entity);
    }

    public Task SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}