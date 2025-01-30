namespace YMS.DTO.Validators
{
	public class InvoiceItemDtoValidator:AbstractValidator<InvoiceItemDto>
	{
		public InvoiceItemDtoValidator() {
			RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("ProductId must be informed");
			RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0).WithMessage("Quantity must be informed");
		}
	}
}
