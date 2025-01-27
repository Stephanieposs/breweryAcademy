
using YMS.Abstractions;

namespace YMS.Services
{
	public class CheckInService(ICheckInRepository repository, IMapper mapper)
	{
		public async Task<CreateCheckInResponse> CreateCheckIn(CreateCheckInBody request)
		{
			var checkIn = mapper.Map<CheckIn>(request);
			var result = await repository.CreateCheckIn(checkIn);
			var checkInId = mapper.Map<CreateCheckInResponse>(result);
			return checkInId;
		}
	
	}
}
