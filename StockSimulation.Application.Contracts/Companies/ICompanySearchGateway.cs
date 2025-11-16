

using StockSimulation.Domain.Companies;

namespace StockSimulation.Application.Contracts.Companies;


public interface ICompanySearchGateway
{
    public Task<IEnumerable<Company>> SearchBySymbol(string symbol);
    public Task<IEnumerable<Company>> SearchByName(string name);
}