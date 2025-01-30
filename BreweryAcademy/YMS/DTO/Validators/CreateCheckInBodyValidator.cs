
namespace YMS.DTO.Validators
{
	public class CreateCheckInBodyValidator:AbstractValidator<CreateCheckInBody>
	{
		public CreateCheckInBodyValidator() {
			RuleFor(x => x.DriverDocument).NotEmpty().WithMessage("DriverDocument is required");
			RuleFor(x => x.TruckPlate).NotEmpty().WithMessage("TruckPlate is required");
			RuleFor(x => x.Invoice).SetValidator(new InvoiceDtoValidator());
		}
	}
}
