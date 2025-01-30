namespace YMS.Abstractions
{
	public interface IInvoiceRepository
	{
		Task<Invoice?> GetInvoiceById(int id);
		Task<Invoice?> GetInvoiceByExternalId(int id);
		Task<List<Invoice>> GetAll();
		InvoiceStatus[] GetInvoiceTypes();
		Task<Invoice> UpdateInvoiceStatus(Invoice invoice);

	}
}
