namespace StockSimulation.Stocks.Shared.Companies;

public class CompanyDto
{
    public required string CompanyName { get; set; }
    public required string Symbol { get; set; }
    public required string Currency { get; set; }
    public required string ExchangeName { get; set; }
    public required string ExchangeSymbol { get; set; }
}