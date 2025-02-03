using YMS.DTO.Invoice.GetAllInvoices;
using YMS.DTO.Invoice.GetInvoice;
using YMS.DTO.Invoice.UpdateInvoice;
using YMS.Exceptions;

namespace YMS.Services
{
	public class InvoiceService(IInvoiceRepository repository, IMapper mapper)
	{
		public async Task<UpdateInvoiceResult> UpdateInvoice(UpdateInvoiceCommand command, int id)
		{
			var validator = new UpdateInvoiceValidator();
			var isValid = await validator.ValidateAsync(command);
			if (!isValid.IsValid) {
				throw new ValidationException(isValid.Errors);
			}

			var invoice = await repository.GetInvoiceByExternalId(id);
			if(invoice is null)
			{
				throw new InvoiceNotFoundException(id);
			}
			var enumValue = EnumHelper.GetValueFromDescription<InvoiceStatus>(command.Status);

			invoice.InvoiceStatus = enumValue;
			var invoiceUpdated = await repository.UpdateInvoiceStatus(invoice); //I want to let this behaviour happen but not to touch the database, just return the status altered to compare
			var result = mapper.Map<UpdateInvoiceResult>(invoiceUpdated);
			return result;
		}
		public async Task<GetInvoiceByIdResult> GetInvoiceById(GetInvoiceByIdRequest request)
		{
			var invoice = await repository.GetInvoiceByExternalId(request.Id);
			if (invoice is null) throw new InvoiceNotFoundException(request.Id);
			var result = mapper.Map<GetInvoiceByIdResult>(invoice);

			return result;
		}

		public async Task<GetAllInvoicesResponse> GetInvoices()
		{
			var invoices = await repository.GetAll();
			List<GetInvoiceByIdResult> result = new List<GetInvoiceByIdResult>();
			foreach (var invoice in invoices) { 
				var invoiceDto = mapper.Map<GetInvoiceByIdResult>(invoice);
				result.Add(invoiceDto);
			}
			return new GetAllInvoicesResponse
			{
				Items = result
			};
		}
	}
}
