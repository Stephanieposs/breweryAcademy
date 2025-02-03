using Microsoft.EntityFrameworkCore;
using SAP4.Entities;

namespace SAP4.Repositories;

public class SapRepository : ISapRepository
{
    private readonly DefaultContext _dContext;

    public SapRepository(DefaultContext dContext)
    {
        _dContext = dContext;
    }

    public async Task<InvoiceSAP> AddInvoice(InvoiceSAP invoice)
    {
        await _dContext.InvoiceSAPs.AddAsync(invoice);
        await _dContext.SaveChangesAsync();

        return invoice;
    }

    public async Task<IEnumerable<InvoiceSAP>> GetAllInvoices()
    {
        return await _dContext.InvoiceSAPs.ToListAsync();
    }

    public async Task<InvoiceSAP> GetById(int id)
    {
        return await _dContext.InvoiceSAPs
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<InvoiceSAP> UpdateInvoiceStatus(InvoiceSAP invoice)
    {
        if (invoice != null)
        {
            _dContext.InvoiceSAPs.Update(invoice);
            await _dContext.SaveChangesAsync();
        }

        return invoice;
    }
}
