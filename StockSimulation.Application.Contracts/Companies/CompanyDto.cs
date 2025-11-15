namespace StockSimulation.Application.Contracts.Companies;

public class CompanyDto
{
    public string CompanyName { get; set; }
    public string Symbol { get; set; }
    public string Currency { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeSymbol { get; set; }
    //
    // // navigation properties
    // public IEnumerable<StockPrice> StockPrices { get; set; }
}