using MassTransit;
using SAP4.Entities;
using SAP4.Repositories;
//using SAP4.SapInvoiceProcessor.BackgoundQueue;

namespace SAP4.Services;

public class SapService : ISapService
{
    public readonly ISapRepository _repo;

    public SapService(ISapRepository repo )
    {
        _repo = repo;
    }
    public async Task AddAsync(InvoiceSAP invoice)
    {
        await _repo.AddInvoice(invoice);
    }

    public async Task<IEnumerable<InvoiceSAP>> GetAllAsync()
    {
        return await _repo.GetAllInvokes();
    }

    public async Task<InvoiceSAP> GetByIdAsync(int id)
    {
        return await _repo.GetById(id);
    }

    public async Task UpdateStatusAsync(InvoiceSAP invoice)
    {
        var existingInvoice = await _repo.GetById(invoice.Id);

        if (existingInvoice == null)
        {
            throw new KeyNotFoundException($"Invoice with ID {invoice.Id} not found.");
        }


        await _repo.UpdateInvoiceStatus(existingInvoice); 

    }
}
