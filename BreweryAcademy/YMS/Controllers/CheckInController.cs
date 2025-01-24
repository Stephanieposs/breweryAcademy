using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YMS.DTO;
using YMS.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YMS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CheckInController(CheckInService service) : ControllerBase
	{

		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<ValuesController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<ValuesController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateCheckInBody request)
		{
			try
			{
				var result = service.CreateCheckIn(request);
				return Created($"CheckIn/{result.Id}", result);
			}
			catch(Exception ex)
			{
				return BadRequest(ex);
			}
		}

		

	}
}
