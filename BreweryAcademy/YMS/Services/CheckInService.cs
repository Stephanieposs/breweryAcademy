using YMS.Data.MessageBroker.Messages;
using YMS.DTO.Validators;
using YMS.DTO.WMSCommunication;
using YMS.Exceptions;

namespace YMS.Services
{
	public class CheckInService(ICheckInRepository repository, IMapper mapper ,HttpClient httpClient, IConfiguration _configuration, IInvoiceRepository invoiceRepository, IBus bus)
	{
		public async Task<CreateCheckInResponse> CreateCheckIn(CreateCheckInBody request)
		{
			var validator = new CreateCheckInBodyValidator();
			var isValid = await validator.ValidateAsync(request);
			if (!isValid.IsValid) throw new ValidationException(isValid.Errors);

			var isInvoiceAlreadyCreated = await invoiceRepository.GetInvoiceByExternalId(request.Invoice.InvoiceId);
			if (isInvoiceAlreadyCreated is not null) throw new BadRequestException("Invoice has already been registered");


			var validationFromSap = await ValidateSapInvoice(request.Invoice.InvoiceId);

			if (!validationFromSap)
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
			checkIn.Invoice.InvoiceStatus = InvoiceStatus.Active;
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

		protected virtual async Task<bool> SendWMSStockExchange(StockMovement stock)
		{
			/*var wmsUrl = _configuration["Urls:WMS"];
			var wmsNewRequestUrl = $"{wmsUrl}/Stock";
			if (wmsUrl is null) throw new InternalServerErrorException("WMS URL not configured");
			var requestToWms = await httpClient.PostAsJsonAsync(wmsNewRequestUrl, stock);*/
			var eventPublisher = new StockUpdateEvent(stock.InvoiceId, stock.OperationType, stock.Products);
			await bus.Publish(eventPublisher);

			return true;
		
		}
		protected virtual async Task<bool> ValidateSapInvoice(int id)
		{
			/*var sapBaseUrl = _configuration["Urls:SAP"];
			if (sapBaseUrl is null) throw new InternalServerErrorException("SAP URL not configured");

			string sapUrl = $"{sapBaseUrl}/api/Sap/yms/{id}";
			var validationFromSap = await httpClient.GetAsync(sapUrl);
			*/
			return true;
		}

	}
}
