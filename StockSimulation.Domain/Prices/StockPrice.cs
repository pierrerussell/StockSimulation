using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StockSimulation.Domain.Prices;


public class StockPrice
{

    public string StockSymbol { get; set; }

    public string ExchangeSymbol { get; set; }
    
    // fmp only gives me end of day price!

    public DateOnly Date { get; set; }
    
    public float Price { get; set; }

    protected StockPrice(){ }

    public StockPrice(string stockSymbol, string exchangeSymbol, float price, DateOnly date)
    {
        StockSymbol = stockSymbol;
        ExchangeSymbol = exchangeSymbol;
        Price = price;
        Date = date;
    }
    
    
    
}