namespace StockSimulation.Stocks.Shared.Companies;

public interface ICompanySearchService
{
    public Task<IEnumerable<CompanyDto>> SearchBySymbol(string symbol);
    public Task<IEnumerable<CompanyDto>> SearchByName(string name);
}