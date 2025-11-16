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

    public Task<IEnumerable<StockPrice>> UpsertManyAsync(IEnumerable<StockPrice> entities)
    {
        throw new NotImplementedException();
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