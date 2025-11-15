namespace StockSimulation.Database.Shared.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<IEnumerable<T>> UpsertManyAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
}