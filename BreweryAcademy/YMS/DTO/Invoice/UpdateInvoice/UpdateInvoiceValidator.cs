namespace YMS.DTO.Invoice.UpdateInvoice
{
	public class UpdateInvoiceValidator:AbstractValidator<UpdateInvoiceCommand>
	{
		public UpdateInvoiceValidator()
		{
			RuleFor(x => x.Status).NotEqual("Unknown");
			RuleFor(x => x.Status).Must(EnumHelper.IsValidEnumDescription<InvoiceStatus>).WithMessage("Invoice Status must be a valid one");
		}
	}
}
