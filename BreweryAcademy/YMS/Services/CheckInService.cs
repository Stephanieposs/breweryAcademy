
using YMS.DTO.WMSCommunication;
using YMS.Exceptions;

namespace YMS.Services
{
	public class CheckInService(ICheckInRepository repository, IMapper mapper ,HttpClient httpClient, IConfiguration _configuration)
	{
		public async Task<CreateCheckInResponse> CreateCheckIn(CreateCheckInBody request)
		{
			var sapBaseUrl = _configuration["Urls:SAP"];
			if (sapBaseUrl is null) throw new Exception("SAP URL not configured");

			string sapUrl = $"{sapBaseUrl}/api/Sap/yms/{request.Invoice.InvoiceId}";
			var validationFromSap = await httpClient.GetAsync(sapUrl);
			if (!validationFromSap.IsSuccessStatusCode)
			{
				throw new BadRequestException("Invoice is not valid");
			}
			var stockTransfer = mapper.Map<StockMovement>(request.Invoice);
			var wasStockSentToWMS = await SendWMSStockExchange(stockTransfer);

			if (!wasStockSentToWMS)
			{
				throw new BadRequestException("Error to send request to WMS");
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
				throw new CheckInNotFoundException(id);
			}
			var result = mapper.Map<GetCheckIn>(checkIn);
			return result;
		}

		private async Task<bool> SendWMSStockExchange(StockMovement stock)
		{
			var wmsUrl = _configuration["Urls:WMS"];
			var wmsNewRequestUrl = $"{wmsUrl}/Stock";
			if (wmsUrl is null) throw new Exception("WMS URL not configured");
			var requestToWms = await httpClient.PostAsJsonAsync(wmsNewRequestUrl, stock);	
			return requestToWms.IsSuccessStatusCode;
		
		}

	}
}
