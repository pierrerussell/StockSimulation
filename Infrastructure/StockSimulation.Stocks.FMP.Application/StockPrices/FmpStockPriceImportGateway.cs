using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockSimulation.Application.Contracts.StockPrices;
using StockSimulation.Domain.StockPrices;
using StockSimulation.Stocks.FMP.Application.Contracts.Configurations;
using StockSimulation.Stocks.FMP.Domain.Companies;
using StockSimulation.Stocks.FMP.Domain.StockPrices;

namespace StockSimulation.Stocks.FMP.Application.StockPrices;

public class FmpStockPriceImportGateway : IStockPriceImportGateway
{
    private readonly ILogger<FmpStockPriceImportGateway> _logger;
    private readonly HttpClient _httpClient;
    private readonly IOptions<FMPOptions> _fmpOptions;

    public FmpStockPriceImportGateway(ILogger<FmpStockPriceImportGateway> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<FMPOptions> fmpOptions)
    {
        _logger = logger;
        _fmpOptions = fmpOptions;
        _httpClient = httpClientFactory.CreateClient(_fmpOptions.Value.HttpClientName);
    }

    public async Task<IEnumerable<StockPrice>> GetStockPricesAsync(string symbol, DateOnly fromDate)
    {
        // just go ahead and get 5 years worth lol
        var fromDateString = fromDate.ToString("yyyy-MM-dd");
        
        _logger.LogInformation("Attempting to pull stock prices of {symbol} from {date}", symbol, fromDate);
        string url = "historical-price-eod/full?symbol=" + symbol
            + "&from=" + fromDateString;
        url = url.AddApiKey(_fmpOptions.Value.ApiKey);
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var stockPrices = JsonSerializer.Deserialize<List<FmpStockPrice>>(content);
        
        if (stockPrices == null)
        {
            return new List<StockPrice>();
        }
        
        return stockPrices.Select(x => new StockPrice
        {
            StockSymbol = symbol,
            Date = DateOnly.FromDateTime(DateTime.Parse(x.Date)),
            Open = x.Open,
            Close = x.Close,
            High = x.High,
            Low = x.Low,
            Volume = x.Volume,
            Change = x.Change,
            ChangePercent = x.ChangePercent,
            Vwap = x.Vwap
        });
    }
    
    
}

public static class StringExtensions
{
    public static string AddApiKey(this string url, string apiKey)
    { 
        return url + "&apikey=" + apiKey;
    }
}