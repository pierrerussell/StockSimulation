
using Microsoft.EntityFrameworkCore;
using StockSimulation.Domain.Companies;
using StockSimulation.Domain.Prices;

namespace StockSimulation.EfCore;

public class ApplicationDbContext : DbContext
{
    
    public DbSet<Company> Companies { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Company>()
            .HasKey(x => new { x.Symbol, x.ExchangeSymbol });
        modelBuilder.Entity<Company>()
            .HasMany(x => x.StockPrices);
        
        modelBuilder.Entity<StockPrice>()
            .HasKey(x => new {x.StockSymbol, x.ExchangeSymbol, x.Date});
       


    }
}