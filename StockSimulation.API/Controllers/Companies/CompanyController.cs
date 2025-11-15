using Microsoft.AspNetCore.Mvc;
using StockSimulation.Application.Contracts.Companies;

namespace StockSimulation.API.Controllers.Companies;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyAppService _companyAppService;
    private readonly ILogger<CompanyController> _logger;
    
    public  CompanyController(ICompanyAppService companyAppService,
        ILogger<CompanyController> logger)
    {
        _logger = logger;
        _companyAppService = companyAppService;
    }

    [HttpGet]
    public async Task<IEnumerable<CompanyDto>> GetBySymbol([FromQuery] string symbol)
    {
        _logger.LogInformation($"Getting company symbol {symbol}");
        
        return await _companyAppService.GetBySymbol(
            symbol
        );
    }
    
    
}