namespace YMS.DTO.Invoice.GetInvoice
{
	public class GetInvoiceByIdProfile:Profile
	{
		public GetInvoiceByIdProfile()
		{
			CreateMap<Entities.Invoice, GetInvoiceByIdResult>().ReverseMap();
		}
	}
}
