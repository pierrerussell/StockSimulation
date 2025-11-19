using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockSimulation.Application.Contracts;
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
        // look in db for the given stock.
        var companyQueryable = _companyRepository.GetQueryable();
        var company = await companyQueryable
            .FirstOrDefaultAsync(x => x.Symbol == symbol); 
         
        if  (company == null) throw new Exception("Company not found");
        
        var stockPriceQueryable = _stockPriceRepository.GetQueryable();
        
        // check if the earliest stock price retrieved was yesterday. if not, 
        // pull in the necessary
        var latestStockPrice = await stockPriceQueryable
            .Where(x => x.StockSymbol == symbol)
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync();
        if (latestStockPrice != null && latestStockPrice.Date == DateOnly.FromDateTime(DateTime.Today.AddDays(-1)))
        {
            var stockPrices = await stockPriceQueryable
                .Where(x => x.Date >= DateOnly.FromDateTime(DateTime.Today).AddYears(-years))
                .ToListAsync();

            var stockPriceDtos = stockPrices
                .Select(x => new StockPriceDto()
                {
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    StockSymbol = x.StockSymbol,
                    Close = x.Close,
                    Open = x.Open,
                    Date = x.Date,
                });

            return stockPriceDtos;
        }

        var pullFromDate = latestStockPrice != null ?
            latestStockPrice.Date.AddDays(1) :
            DateOnly.FromDateTime(DateTime.Today.AddYears(-5));
        
        _logger.LogInformation($"couldnt find stockprices in db, hitting fmp");
        var prices = await _stockPriceImportGateway.GetStockPricesAsync(symbol, pullFromDate);
        await _stockPriceRepository.UpsertManyAsync(prices);
        await _stockPriceRepository.SaveChangesAsync();
        return prices
            .OrderByDescending(p => p.Date)
            .Select(x => new StockPriceDto()
            {
                High = x.High,
                Low = x.Low,
                Volume = x.Volume,
                StockSymbol = x.StockSymbol,
                Close = x.Close,
                Open = x.Open,
                Date = x.Date
            });




    }
}