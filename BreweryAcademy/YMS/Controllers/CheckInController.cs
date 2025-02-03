using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YMS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CheckInController(CheckInService service) : ControllerBase
	{
		[ProducesResponseType(typeof(GetAllCheckInsResponse), StatusCodes.Status200OK)]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			
				var result = await service.GetAllCheckIns();
				return Ok(result);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetCheckIn), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			
			var response = await service.GetCheckIn(id);
			return Ok(response);
			
			
		}

		[HttpPost]
		[ProducesResponseType(typeof(CreateCheckInResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Post([FromBody] CreateCheckInBody request)
		{
			
			var result = await service.CreateCheckIn(request);
             return Created($"/CheckIn/{result.Id}", new CreateCheckInResponse
                {
                    Id = result.Id
                });
            
		}

		

	}
}
