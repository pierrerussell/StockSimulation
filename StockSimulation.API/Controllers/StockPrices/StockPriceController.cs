using Microsoft.AspNetCore.Mvc;
using StockSimulation.Application.Contracts;
using StockSimulation.Application.Contracts.StockPrices;
using StockSimulation.Domain.StockPrices;

namespace StockSimulation.API.Controllers.StockPrices;

[ApiController]
[Route("api/stock-prices")]
public class StockPriceController : ControllerBase
{
    private readonly ILogger<StockPriceController> _logger;
    private readonly IStockPriceAppService  _stockPriceAppService;
    

    public StockPriceController(
        ILogger<StockPriceController> logger,
        IStockPriceAppService  stockPriceAppService
        )
    {
        _logger = logger;
        _stockPriceAppService = stockPriceAppService;
    }

    // return past 2 years of stock prices to user
    [HttpGet("{symbol}")]
    public async Task<ActionResult<IEnumerable<StockPriceDto>>> Get(string symbol)
    {
        return Ok(await _stockPriceAppService.GetStockPrices(symbol));
    }
    
    
    
    
    
    
}