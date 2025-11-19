namespace StockSimulation.Domain.StockPrices;


public class StockPrice
{

    public required string StockSymbol { get; set; }
    
    // fmp only gives me end of day price!
    public required DateOnly Date { get; set; }
    
    public required decimal Open { get; set; }
    
    public required decimal Close { get; set; }
    public required decimal High { get; set; }
    public required decimal Low { get; set; }
    public required int Volume { get; set; }
    public required decimal Change { get; set; }
    public required decimal ChangePercent { get; set; }
    public decimal Vwap { get; set; }
    
    
    public StockPrice(){ }
    
    
}