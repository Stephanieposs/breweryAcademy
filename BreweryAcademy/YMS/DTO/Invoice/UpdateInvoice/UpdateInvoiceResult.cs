namespace YMS.DTO.Invoice.UpdateInvoice
{
	public class UpdateInvoiceResult
	{
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public List<InvoiceItemDto> Items { get; set; } = new();
		public string InvoiceType { get; set; } = string.Empty;
		public string InvoiceStatus { get; set; } = string.Empty;
	}
}
