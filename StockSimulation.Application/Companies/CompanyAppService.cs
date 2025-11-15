using Microsoft.Extensions.Logging;
using StockSimulation.Application.Contracts.Companies;
using StockSimulation.Domain.Companies;
using StockSimulation.Stocks.Shared.Companies;
using CompanyDto = StockSimulation.Application.Contracts.Companies.CompanyDto;

namespace StockSimulation.Application.Companies;

public class CompanyAppService : ICompanyAppService
{
    private readonly ILogger<CompanyAppService> _logger;
    private readonly ICompanySearchService _companySearchService;
    private readonly ICompanyRepository _companyRepository;

    public CompanyAppService(
        ILogger<CompanyAppService> logger,
        ICompanySearchService companySearchService,
        ICompanyRepository companyRepository)
    {
        _companySearchService = companySearchService;
    }


    public async Task<IEnumerable<CompanyDto>> GetBySymbol(string symbol)
    {
        // search in db
        // if in db return from there
        var queryable = _companyRepository.GetQueryable();
        var companies = queryable
            .Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
            .Where(company => company.Symbol == symbol)
            .ToList();
        if (companies.Count != 0)
        {
            return companies;
        }
        
        // else search from fmp, throw in db
        var externalCompanies = await _companySearchService.SearchBySymbol(symbol);
        var newCompanies = externalCompanies.Select(x => new Company(
            x.CompanyName,
            x.Symbol,
            x.Currency,
            x.ExchangeName,
            x.ExchangeSymbol
        ));
        // save in db then return;
        var result = await _companyRepository.UpsertManyAsync(newCompanies);
        companies = result.Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
            .Where(company => company.Symbol == symbol)
            .ToList();
        return companies;
    }

    public Task<IEnumerable<CompanyDto>> GetByName(string name)
    {
        throw new NotImplementedException();
    }
}