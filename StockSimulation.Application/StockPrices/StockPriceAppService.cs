using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockSimulation.Application.Contracts.StockPrices;
using StockSimulation.Domain.Companies;
using StockSimulation.Domain.StockPrices;

namespace StockSimulation.Application.StockPrices;

public class StockPriceAppService : IStockPriceAppService
{
    
    private readonly IStockPriceRepository _stockPriceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IStockPriceImportGateway _stockPriceImportGateway;
    private readonly ILogger<StockPriceAppService> _logger;
    
    public StockPriceAppService(
        ILogger<StockPriceAppService> logger,
        IStockPriceRepository stockPriceRepository,
        ICompanyRepository companyRepository,
        IStockPriceImportGateway stockPriceImportGateway
        )
    {
        _logger = logger;
        _stockPriceRepository = stockPriceRepository;
        _companyRepository = companyRepository;
        _stockPriceImportGateway = stockPriceImportGateway;
    }
    
    public async Task<IEnumerable<StockPriceDto>> GetStockPrices(string symbol, int years = 2)
    {
        // check the company exists
        var company = await _companyRepository.GetQueryable()
            .FirstOrDefaultAsync(x => x.Symbol == symbol);
        if (company == null) throw new Exception($"Company not found: {symbol}");

        // if latest stock price date is < yesterday, pull new data from gateway
        await EnsureDataIsFresh(symbol);
        
        var cutoffDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-years));
        var stockPrices = await _stockPriceRepository.GetQueryable()
            .Where(x => x.StockSymbol == symbol && x.Date >= cutoffDate)
            .OrderByDescending(x => x.Date)
            .ToListAsync();

        return MapToDto(stockPrices);
    }

    private async Task EnsureDataIsFresh(string symbol)
    {
        var latestPrice = await _stockPriceRepository.GetQueryable()
            .Where(x => x.StockSymbol == symbol)
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync();

        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        if (latestPrice?.Date >= yesterday) return;

        var pullFromDate = latestPrice?.Date.AddDays(1) 
            ?? DateOnly.FromDateTime(DateTime.Today.AddYears(-5));

        _logger.LogInformation($"Updating stock prices for {symbol} from {pullFromDate}");
        var newPrices = await _stockPriceImportGateway.GetStockPricesAsync(symbol, pullFromDate);
        await _stockPriceRepository.UpsertManyAsync(newPrices);
        await _stockPriceRepository.SaveChangesAsync();
    }

    private IEnumerable<StockPriceDto> MapToDto(IEnumerable<StockPrice> prices)
    {
        return prices.Select(x => new StockPriceDto
        {
            StockSymbol = x.StockSymbol,
            Date = x.Date,
            Open = x.Open,
            Close = x.Close,
            High = x.High,
            Low = x.Low,
            Volume = x.Volume
        });
    }
}