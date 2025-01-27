
using YMS.Exceptions;

namespace YMS.Services
{
	public class CheckInService(ICheckInRepository repository, IMapper mapper ,HttpClient httpClient)
	{
		public async Task<CreateCheckInResponse> CreateCheckIn(CreateCheckInBody request)
		{
			string sapUrl = $"https://localhost:7046/api/Sap/yms/{request.Invoice.InvoiceId}";
			var validationFromSap = await httpClient.GetAsync(sapUrl);
			if (!validationFromSap.IsSuccessStatusCode)
			{
				throw new Exception("Invoice is not valid");
			}

			var checkIn = mapper.Map<CheckIn>(request);
			var result = await repository.CreateCheckIn(checkIn);
			var checkInId = mapper.Map<CreateCheckInResponse>(result);
			return checkInId;
		}

		public async Task<GetAllCheckInsResponse> GetAllCheckIns()
		{
			var listOfCheckInsResponse = new GetAllCheckInsResponse
			{
				Items = new()
			};
			var checkIns = await repository.GetAllCheckIns();
			foreach (var check in checkIns) { 
				var checkInDto = mapper.Map<GetCheckIn>(check);
				listOfCheckInsResponse.Items.Add(checkInDto);
			}

			return listOfCheckInsResponse;
		}

		public async Task<GetCheckIn> GetCheckIn (int id) {
			
			var checkIn = await repository.GetCheckIn(id);	
			if(checkIn is null)
			{
				throw new NotFoundException("CheckIn", id);
			}
			var result = mapper.Map<GetCheckIn>(checkIn);
			return result;
		}

	}
}
