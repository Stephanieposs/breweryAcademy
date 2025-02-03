using BuildingBlocks.Exceptions;
using MassTransit;
using SAP4.Entities;
using SAP4.Repositories;

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
        if (invoice == null)
        {
            throw new BadRequestException(nameof(invoice));
        }

        await _repo.AddInvoice(invoice);
    }

    public async Task<IEnumerable<InvoiceSAP>> GetAllAsync()
    {
        return await _repo.GetAllInvoices();
    }

    public async Task<InvoiceSAP> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new BadRequestException("Invalid invoice ID.");
        }

        return await _repo.GetById(id);
    }

    public async Task UpdateStatusAsync(InvoiceSAP invoice)
    {
        if (invoice == null)
        {
            throw new BadRequestException(nameof(invoice));
        }

        if (invoice.Id <= 0)
        {
            throw new BadRequestException("Invalid invoice ID.");
        }

        var existingInvoice = await _repo.GetById(invoice.Id);

        if (existingInvoice == null)
        {
            throw new NotFoundException($"Invoice with ID {invoice.Id} not found.");
        }

        await _repo.UpdateInvoiceStatus(existingInvoice); 
    }
}
