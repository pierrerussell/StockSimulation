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


    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> Search([FromQuery] string? symbol = null, [FromQuery] string? name = null)
    {
        _logger.LogInformation($"Searching for companies {symbol} {name}");
        if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(name))
            return BadRequest("Provide only either symbol OR name");
        
        if (!string.IsNullOrEmpty(symbol))
            return Ok(await _companyAppService.GetBySymbol(symbol));

        if (!string.IsNullOrEmpty(name))
            return Ok(await _companyAppService.GetByName(name));

        return BadRequest("Provide either symbol or name");
    }
    
}