
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockSimulation.Application.Companies;
using StockSimulation.Application.Contracts.Companies;
using StockSimulation.Database.Shared.Repositories;
using StockSimulation.Domain.Companies;
using StockSimulation.EfCore;
using StockSimulation.EfCore.Companies;
using StockSimulation.Stocks.FMP.Application.Companies;
using StockSimulation.Stocks.FMP.Application.Contracts.Configurations;
using StockSimulation.Stocks.Shared.Companies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// yall thought id let you see my api key in appsettings
builder.Configuration.AddJsonFile(
    path: "secrets.json",
    optional: true,
    reloadOnChange: true);
var fmpOptions = builder.Configuration.GetSection(FMPOptions.SectionName).Get<FMPOptions>();
builder.Services.AddHttpClient(fmpOptions.HttpClientName, x =>
{
    x.BaseAddress = new Uri(fmpOptions.BaseUrl);
});

// db set up
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");
Directory.CreateDirectory(dataDirectory);
var dbPath = Path.Combine(dataDirectory, "stocksim.db");
connectionString = $"Data Source={dbPath}";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("StockSimulation.API"));
});


builder.Services.AddScoped<ICompanySearchService, FMPCompanySearchService>();
builder.Services.AddScoped<ICompanyAppService, CompanyAppService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = builder.Configuration;
builder.Services.Configure<FMPOptions>(
    configuration.GetSection(FMPOptions.SectionName)
    );

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();