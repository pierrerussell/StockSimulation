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
        companies.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Company> UpdateAsync(Company entity)
    {
        var  companies =  _dbContext.Companies;
        companies.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Company>> UpsertManyAsync(IEnumerable<Company> entities)
    {
        foreach (var company in entities)
        {
            var existing = await _dbContext.Companies.FirstOrDefaultAsync(c =>
                c.Symbol == company.Symbol && c.ExchangeSymbol == company.ExchangeSymbol);

            if (existing != null)
            {
                _dbContext.Entry(existing).CurrentValues.SetValues(company);
            }
            else
            {
                await _dbContext.AddAsync(company);
            }
        }

        await _dbContext.SaveChangesAsync();
        return entities;
    }

    public Task DeleteAsync(Company entity)
    {
        _dbContext.Companies.Remove(entity);
        return _dbContext.SaveChangesAsync();
    }
}