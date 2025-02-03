using SAP4.Entities;

namespace SAP4.Services;

public interface ISapService
{
    Task<InvoiceSAP> GetByIdAsync(int id);
    Task<IEnumerable<InvoiceSAP>> GetAllAsync();
    Task AddAsync(InvoiceSAP invoice);
    Task UpdateStatusAsync(InvoiceSAP invoice);
    //Task ProcessInvoiceAsync(InvoiceSAP invoice);
}
