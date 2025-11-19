namespace StockSimulation.Application.Contracts.StockPrices;

public interface IStockPriceAppService
{
    
    public Task<IEnumerable<StockPriceDto>> GetStockPrices(string symbol, int years = 2);
    
}