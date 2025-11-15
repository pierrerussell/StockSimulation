using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockSimulation.Stocks.FMP.Application.Contracts.Configurations;
using StockSimulation.Stocks.FMP.Domain.Companies;
using StockSimulation.Stocks.Shared.Companies;

namespace StockSimulation.Stocks.FMP.Application.Companies;

public class FMPCompanySearchService : ICompanySearchService
{
    
    private IOptions<FMPOptions> _fmpOptions;
    private readonly HttpClient _httpClient;
    private readonly ILogger<FMPCompanySearchService> _logger;
    public FMPCompanySearchService(
        IOptions<FMPOptions> fmpOptions, 
        IHttpClientFactory httpClientFactory,
        ILogger<FMPCompanySearchService> logger)
    {
        _fmpOptions = fmpOptions;
        _httpClient = httpClientFactory.CreateClient(_fmpOptions.Value.HttpClientName);
        _logger = logger;
    }
    
    public async Task<IEnumerable<CompanyDto>> SearchBySymbol(string symbol)
    {
        _logger.LogInformation($"Searching for {symbol}");
        string url = "search-symbol?query=" + symbol;
        url = url.AddApiKey(_fmpOptions.Value.ApiKey);
        Console.WriteLine(_httpClient.BaseAddress);
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation(content);
        var companies = JsonSerializer.Deserialize<List<Company>>(content);
        var companyDtos = companies
            .Select(x => new CompanyDto
            {
                Symbol = x.Symbol,
                CompanyName = x.Name,
                Currency = x.Currency,
                ExchangeName = x.ExchangeFullName,
                ExchangeSymbol = x.Exchange
            });
        
        
        return companyDtos;
    }

    public Task<IEnumerable<CompanyDto>> SearchByName(string name)
    {
        throw new NotImplementedException();
    }
    
}

public static class StringExtensions
{
    public static string AddApiKey(this string url, string apiKey)
    { 
        return url + "&apikey=" + apiKey;
    }
}