namespace YMS.DTO.Validators
{
	public class InvoiceDtoValidator:AbstractValidator<InvoiceDto>
	{
		public InvoiceDtoValidator() {
			RuleFor(x => x.InvoiceId).GreaterThanOrEqualTo(0).NotEmpty().WithMessage("InvoiceId must be valid");
			RuleFor(x => x.InvoiceType).Must(EnumHelper.IsValidEnumDescription<InvoiceType>).WithMessage("Invoice Type must be valid");
		
			RuleFor(x => x.Items).NotEmpty().ForEach(x => x.SetValidator(new InvoiceItemDtoValidator()));
		}
	}
}
