
using Microsoft.EntityFrameworkCore;
using StockSimulation.Domain.Companies;
using StockSimulation.Domain.StockPrices;

namespace StockSimulation.EfCore;

public class ApplicationDbContext : DbContext
{
    
    public DbSet<Company> Companies { get; set; }
    public DbSet<StockPrice> StockPrices { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Company>()
            .HasKey(x => x.Symbol);
        modelBuilder.Entity<Company>()
            .HasMany(x => x.StockPrices)
            .WithOne()
            .HasForeignKey(sp => sp.StockSymbol);
        
        modelBuilder.Entity<StockPrice>()
            .ToTable("StockPrices")
            .HasKey(x => new {x.StockSymbol, x.Date});

    }
}