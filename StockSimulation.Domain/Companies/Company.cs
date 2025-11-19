using System.ComponentModel.DataAnnotations;
using StockSimulation.Domain.StockPrices;

namespace StockSimulation.Domain.Companies;

public class Company
{
    [Key]
    public string Symbol { get; set; }
    
    public string CompanyName { get; set; }
    public string Currency { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeSymbol { get; set; }
    
    // navigation properties
    public IEnumerable<StockPrice> StockPrices { get; set; }
    
    protected Company() {}

    public Company(string symbol, string companyName, string currency, string exchangeName, string exchangeSymbol)
    {
        Symbol = symbol;
        CompanyName = companyName;
        Currency = currency;
        ExchangeName = exchangeName;
        ExchangeSymbol = exchangeSymbol;
    }
}