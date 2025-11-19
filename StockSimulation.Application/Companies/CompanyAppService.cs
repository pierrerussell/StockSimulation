using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockSimulation.Application.Contracts.Companies;
using StockSimulation.Domain.Companies;
using CompanyDto = StockSimulation.Application.Contracts.Companies.CompanyDto;

namespace StockSimulation.Application.Companies;

public class CompanyAppService : ICompanyAppService
{
    private readonly ILogger<CompanyAppService> _logger;
    private readonly ICompanySearchGateway _companySearchGateway;
    private readonly ICompanyRepository _companyRepository;

    public CompanyAppService(
        ILogger<CompanyAppService> logger,
        ICompanySearchGateway companySearchGateway,
        ICompanyRepository companyRepository)
    {
        _companySearchGateway = companySearchGateway;
        _companyRepository = companyRepository;
        _logger = logger;
    }


    public async Task<IEnumerable<CompanyDto>> GetBySymbol(string symbol)
    {
        // search in db
        // if in db return from there
        var queryable = _companyRepository.GetQueryable();
        var companies = queryable
            .Where(company =>  EF.Functions.Like(company.Symbol, $"%{symbol}%"))
            .Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
           
            .ToList();
        if (companies.Count != 0)
        {
            return companies;
        }
        
        // else search from fmp, throw in db
        var externalCompanies = await _companySearchGateway.SearchBySymbol(symbol);
        // save in db then return;
        var result = await _companyRepository.UpsertManyAsync(externalCompanies);
        companies = result
            .Where(company => company.Symbol.Contains(symbol, StringComparison.OrdinalIgnoreCase))
            .Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
            .ToList();
        await _companyRepository.SaveChangesAsync();
        return companies;
    }

    public async Task<IEnumerable<CompanyDto>> GetByName(string name)
    {
        var queryable = _companyRepository.GetQueryable();
        var companies = queryable
            .Where(company => EF.Functions.Like(company.CompanyName, $"%{name}%"))
            .Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
            .ToList();
        if (companies.Count != 0)
        {
            return companies;
        }
        
        // else search from fmp, throw in db
        var externalCompanies = await _companySearchGateway.SearchByName(name);
        // save in db then return;
        var result = await _companyRepository.UpsertManyAsync(externalCompanies);
        companies = result
            .Where(company => company.CompanyName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .Select(x => new CompanyDto()
            {
                Symbol = x.Symbol,
                CompanyName = x.CompanyName,
                Currency = x.Currency,
                ExchangeName = x.ExchangeName,
                ExchangeSymbol = x.ExchangeSymbol,
            })
            .ToList();
        await _companyRepository.SaveChangesAsync();
        return companies;
    }
}