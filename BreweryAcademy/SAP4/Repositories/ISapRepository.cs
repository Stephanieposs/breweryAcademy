using SAP4.Entities;

namespace SAP4.Repositories;

public interface ISapRepository
{
    Task<IEnumerable<InvoiceSAP>> GetAllInvokes();
    Task<InvoiceSAP> GetById(int id);
    Task<InvoiceSAP> AddInvoice(InvoiceSAP invoice);
    Task<InvoiceSAP> UpdateInvoiceStatus(InvoiceSAP invoice);
}
