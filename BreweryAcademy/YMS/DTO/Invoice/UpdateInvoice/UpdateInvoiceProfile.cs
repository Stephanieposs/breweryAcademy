namespace YMS.DTO.Invoice.UpdateInvoice
{
	public class UpdateInvoiceProfile:Profile
	{
		public UpdateInvoiceProfile()
		{
			CreateMap<YMS.Entities.Invoice, UpdateInvoiceResult>().ReverseMap();
		}
	}
}
