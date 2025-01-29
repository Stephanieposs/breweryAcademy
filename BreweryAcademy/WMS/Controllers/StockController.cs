using Microsoft.AspNetCore.Mvc;
using WMS.Entities;
using WMS.Interfaces;
using WMS.Services;

namespace WMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : Controller
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockService.GetAllStocks();

            if (stocks == null || !stocks.Any())
            {
                return NotFound(new { Message = "No stocks found." });
            }

            return Ok(stocks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] Stock stock)
        {
            var createdStock = await _stockService.CreateStock(stock);
            return CreatedAtAction(nameof(GetStockById), new { id = createdStock.Id }, createdStock);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await _stockService.GetStockById(id);
            return Ok(stock);
        }
    }
}
