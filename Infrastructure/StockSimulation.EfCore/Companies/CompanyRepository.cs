using Microsoft.EntityFrameworkCore;
using StockSimulation.Domain.Companies;

namespace StockSimulation.EfCore.Companies;

public class CompanyRepository : ICompanyRepository
{

    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Company> GetQueryable()
    { 
        return _dbContext.Companies;
    }

    public async Task<Company> AddAsync(Company entity)
    {
        var companies = _dbContext.Companies;
        await companies.AddAsync(entity);
        return entity;
    }

    public Company Update(Company entity)
    {
        var  companies =  _dbContext.Companies;
        companies.Update(entity);
        return entity;
    }

    public async Task<IEnumerable<Company>> UpsertManyAsync(IEnumerable<Company> entities)
    {
        var companiesList = entities.ToList();
        if (!companiesList.Any())
        {
            return companiesList;
        }

        var keys = companiesList.Select(c => c.Symbol).ToList();
        var existingCompanies = await _dbContext.Companies
            .Where(c => keys.Contains(c.Symbol))
            .ToListAsync();

        // Create a dictionary for quick lookup
        var existingDict = existingCompanies.ToDictionary(c => c.Symbol, c => c);
        
        foreach (var company in companiesList)
        {
            if (existingDict.TryGetValue(company.Symbol, out var existingCompany))
            {
                // Update existing company
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.Currency = company.Currency;
                existingCompany.ExchangeName = company.ExchangeName;
                existingCompany.ExchangeSymbol = company.ExchangeSymbol;
            }
            else
            {
                // Add new company
                await _dbContext.Companies.AddAsync(company);
            }
        }
        
        return companiesList;
    }

    public void Delete(Company entity)
    {
        _dbContext.Companies.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}