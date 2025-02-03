using YMS.DTO.Invoice.GetInvoice;

namespace YMS.DTO.Invoice.GetAllInvoices
{
	public class GetAllInvoicesResponse
	{
		public List<GetInvoiceByIdResult> Items { get; set; } = new();
	}
}
