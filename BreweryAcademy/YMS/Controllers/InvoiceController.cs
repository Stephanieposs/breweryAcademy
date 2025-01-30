using Microsoft.AspNetCore.Mvc;
using YMS.DTO.Invoice.UpdateInvoice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YMS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InvoiceController(InvoiceService service) : ControllerBase
	{
		// GET: api/<InvoiceController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<InvoiceController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}
		// PUT api/<InvoiceController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] UpdateInvoiceCommand value)
		{
			var result = await service.UpdateInvoice(value, id);
			return Ok(result);
		}

	}
}
