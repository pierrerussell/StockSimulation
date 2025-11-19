using StockSimulation.Domain.StockPrices;

namespace StockSimulation.Application.Contracts.StockPrices;

public interface IStockPriceImportGateway
{
    public Task<IEnumerable<StockPrice>> GetStockPricesAsync(string symbol, DateOnly fromDate);
}