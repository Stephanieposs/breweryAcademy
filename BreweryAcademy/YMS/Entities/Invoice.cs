

namespace YMS.Entities
{
	public class Invoice
	{
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
        public InvoiceType InvoiceType { get; set; }
		public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Active;
		public int CheckInId { get; set; }
		public virtual CheckIn CheckIn { get; set; } = default!;

		public Invoice() { }
	}
}
