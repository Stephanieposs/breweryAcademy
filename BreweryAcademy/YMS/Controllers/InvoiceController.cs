using Microsoft.AspNetCore.Mvc;
using YMS.DTO.Invoice.GetInvoice;
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
		[ProducesResponseType(typeof(GetAllCheckInsResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> Get()
		{
			var response = await service.GetInvoices();
			return Ok(response);
		}

		// GET api/<InvoiceController>/5
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetInvoiceByIdResult), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			var newQuery = new GetInvoiceByIdRequest { Id = id };
			var result =await service.GetInvoiceById(newQuery);
			return Ok(result);
		}
		// PUT api/<InvoiceController>/5
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(UpdateInvoiceResult), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateInvoiceCommand value)
		{
			var result = await service.UpdateInvoice(value, id);
			return Ok(result);
		}

	}
}
