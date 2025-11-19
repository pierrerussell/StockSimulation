using System.Text.Json.Serialization;

namespace StockSimulation.Stocks.FMP.Domain.StockPrices;

public class FmpStockPrice
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
    [JsonPropertyName("date")]
    public string Date { get; set; }
    [JsonPropertyName("open")]
    public decimal Open { get; set; }
    [JsonPropertyName("high")]
    public decimal High { get; set; }
    [JsonPropertyName("low")]
    public decimal Low { get; set; }
    [JsonPropertyName("close")]
    public decimal Close { get; set; }
    [JsonPropertyName("volume")]
    public int Volume { get; set; }
    [JsonPropertyName("change")]
    public decimal Change { get; set; }
    [JsonPropertyName("changePercent")]
    public decimal ChangePercent { get; set; }
    [JsonPropertyName("vwap")]
    public decimal Vwap { get; set; }


    
}