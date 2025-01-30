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
			var invoiceUpdated = await repository.UpdateInvoiceStatus(invoice);
			var result = mapper.Map<UpdateInvoiceResult>(invoiceUpdated);
			return result;
		}
	}
}
