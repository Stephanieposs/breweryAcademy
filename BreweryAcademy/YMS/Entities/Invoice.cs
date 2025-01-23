using YMS.Enums;

namespace YMS.Entities
{
	public class Invoice
	{
        public int Id { get; set; }
        public int InvoiceKey { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
        public InvoiceType InvoiceType { get; set; }
	}
}
