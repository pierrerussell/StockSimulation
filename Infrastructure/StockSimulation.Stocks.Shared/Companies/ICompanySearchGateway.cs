namespace StockSimulation.Stocks.Shared.Companies;

public interface ICompanySearchGateway
{
    public Task<IEnumerable<CompanyDto>> SearchBySymbol(string symbol);
    public Task<IEnumerable<CompanyDto>> SearchByName(string name);
}