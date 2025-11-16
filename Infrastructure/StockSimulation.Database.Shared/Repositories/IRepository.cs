namespace StockSimulation.Database.Shared.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task<T> AddAsync(T entity);
    T Update(T entity);
    Task<IEnumerable<T>> UpsertManyAsync(IEnumerable<T> entities);
    void Delete(T entity);
    Task SaveChangesAsync();
}