using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YMS.DTO;
using YMS.Exceptions;
using YMS.Services;

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
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			
			var response = await service.GetCheckIn(id);
			return Ok(response);
			
			
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateCheckInBody request)
		{
			
			var result = await service.CreateCheckIn(request);
             return Ok(new CreateCheckInResponse
                {
                    Id = result.Id
                });
            
		}

		

	}
}
