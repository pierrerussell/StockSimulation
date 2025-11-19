using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockSimulation.Application.Contracts.Companies;
using StockSimulation.Stocks.FMP.Application.Contracts.Configurations;
using StockSimulation.Stocks.FMP.Domain.Companies;

namespace StockSimulation.Stocks.FMP.Application.Companies;

public class FmpCompanySearchGateway : ICompanySearchGateway
{
    private readonly IOptions<FMPOptions> _fmpOptions;
    private readonly HttpClient _httpClient;
    private readonly ILogger<FmpCompanySearchGateway> _logger;
    public FmpCompanySearchGateway(
        IOptions<FMPOptions> fmpOptions, 
        IHttpClientFactory httpClientFactory,
        ILogger<FmpCompanySearchGateway> logger)
    {
        _fmpOptions = fmpOptions;
        _httpClient = httpClientFactory.CreateClient(_fmpOptions.Value.HttpClientName);
        _logger = logger;
    }
    
    public async Task<IEnumerable<StockSimulation.Domain.Companies.Company>> SearchBySymbol(string symbol)
    {
        try
        {
            _logger.LogInformation($"Searching for {symbol}");
            string url = "search-symbol?query=" + symbol;
            url = url.AddApiKey(_fmpOptions.Value.ApiKey);
            Console.WriteLine(_httpClient.BaseAddress);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(content);
            var companies = JsonSerializer.Deserialize<List<FmpCompany>>(content);
            var companyList = companies
                .Select(x => new StockSimulation.Domain.Companies.Company(
                    x.Symbol,
                    x.Name,
                    x.Currency,
                    x.ExchangeFullName,
                    x.Exchange));
            return companyList;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "FMP API request for companies by symbol failed");
            return new List<StockSimulation.Domain.Companies.Company>();
        }
    }

    public async Task<IEnumerable<StockSimulation.Domain.Companies.Company>> SearchByName(string name)
    {
        try
        {
            _logger.LogInformation($"Searching for {name}");
            string url = "search-name?query=" + name;
            url = url.AddApiKey(_fmpOptions.Value.ApiKey);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var companies = JsonSerializer.Deserialize<List<FmpCompany>>(content);
            var companyList = companies
                .Select(x => new StockSimulation.Domain.Companies.Company(
                    x.Symbol,
                    x.Name,
                    x.Currency,
                    x.ExchangeFullName,
                    x.Exchange));
            return companyList;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "FMP API request for companies by name failed");
            return new List<StockSimulation.Domain.Companies.Company>();
        }


    }
    
}

public static class StringExtensions
{
    public static string AddApiKey(this string url, string apiKey)
    { 
        return url + "&apikey=" + apiKey;
    }
}