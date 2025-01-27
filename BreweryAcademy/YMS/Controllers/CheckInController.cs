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
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var result = await service.GetAllCheckIns();
				return Ok(result);
			}
			catch (Exception ex) { 
				return BadRequest(ex.Message);	
			}
		}

		// GET api/<ValuesController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			try
			{
				var response = await service.GetCheckIn(id);
				return Ok(response);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex) {
				return BadRequest(ex.Message);
			}
			
		}

		// POST api/<ValuesController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateCheckInBody request)
		{
			try
			{
				var result = await service.CreateCheckIn(request);
                return Ok(new CreateCheckInResponse
                {
                    Id = result.Id
                });
            }
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		

	}
}
