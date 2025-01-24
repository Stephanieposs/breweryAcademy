using Microsoft.AspNetCore.Mvc;
using WMS.Entities;
using WMS.Interfaces;

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
        public async Task<IActionResult> UpdateStock([FromBody] Stock payload)
        {
            if (payload == null || payload.Products == null || !payload.Products.Any())
                return BadRequest(new { Message = "Invalid payload" });

            if (payload.OperationType != Enums.OperationType.Load && payload.OperationType != Enums.OperationType.Unload)
            {
                return BadRequest(new { Message = "Invalid operation type" });
            }

            var updatedStock = await _stockService.UpdateQuantity(payload);

            if (updatedStock == null)
                return NotFound(new { Message = "Stock not found" });

            return Ok(new { Message = "Stock updated successfully!" });
        }
    }
}
