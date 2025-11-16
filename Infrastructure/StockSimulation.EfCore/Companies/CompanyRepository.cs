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
        var keys = companiesList.Select(c => new { c.Symbol, c.ExchangeSymbol }).ToList();
        var existing = await _dbContext.Companies
            .Where(c => keys.Any(k => k.Symbol == c.Symbol && k.ExchangeSymbol == c.ExchangeSymbol))
            .ToListAsync();
        foreach (var company in companiesList)
        {
            if (existing != null)
            {
                _dbContext.Entry(existing).CurrentValues.SetValues(company);
            }
            else
            {
                await _dbContext.Companies.AddAsync(company);
            }
        }
        
        return entities;
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