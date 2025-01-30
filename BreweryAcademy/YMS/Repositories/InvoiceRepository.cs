
namespace YMS.Repositories
{
	public class InvoiceRepository(DefaultContext context) : IInvoiceRepository
	{
		public async Task<List<Invoice>> GetAll()
		{
			var listOfInvoices = await context.Invoices.Include(x => x.Items).ToListAsync();
			return listOfInvoices;
		}

		public async Task<Invoice?> GetInvoiceByExternalId(int id)
		{
			var invoice = await context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.InvoiceId == id);
			return invoice;
		}

		public async Task<Invoice?> GetInvoiceById(int id)
		{
			var invoice = await context.Invoices.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
			return invoice;
		}

		public InvoiceStatus[] GetInvoiceTypes()
		{
			InvoiceStatus[] invoiceStatusValues =(InvoiceStatus[])Enum.GetValues(typeof(InvoiceStatus));
			return invoiceStatusValues;

		}

		public async Task<Invoice> UpdateInvoiceStatus(Invoice invoice)
		{
			var invoiceToBeUpdated = await GetInvoiceByExternalId(invoice.InvoiceId);
			invoiceToBeUpdated!.InvoiceStatus = invoice.InvoiceStatus;
			await context.SaveChangesAsync();
			return invoiceToBeUpdated;
		}
	}
}
