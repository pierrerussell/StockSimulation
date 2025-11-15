using System.Text.Json.Serialization;

namespace StockSimulation.Stocks.FMP.Domain.Companies;

public class Company
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    [JsonPropertyName("exchangeFullName")]
    public string ExchangeFullName { get; set; }
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; }
}