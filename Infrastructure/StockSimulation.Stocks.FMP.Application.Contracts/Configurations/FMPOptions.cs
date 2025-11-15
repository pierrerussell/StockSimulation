namespace StockSimulation.Stocks.FMP.Application.Contracts.Configurations;

public class FMPOptions
{
    public const string SectionName = "FMP";
    public string ApiKey { get; set; }
    public string HttpClientName  { get; set; }
    public string BaseUrl  { get; set; }
}