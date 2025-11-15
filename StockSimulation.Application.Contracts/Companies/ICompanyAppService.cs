namespace StockSimulation.Application.Contracts.Companies;

public interface ICompanyAppService 
{ 
    Task<IEnumerable<CompanyDto>> GetBySymbol(string symbol);
    Task<IEnumerable<CompanyDto>> GetByName(string name);
    
}