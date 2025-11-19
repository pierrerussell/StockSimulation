namespace StockSimulation.Application.Contracts.StockPrices;

public class StockPriceDto
{
    public string StockSymbol { get; set; }
    
    // fmp only gives me end of day price!
    public DateOnly Date { get; set; }
    
    public decimal Open { get; set; }
    
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public int Volume { get; set; }


}