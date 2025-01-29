using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SAP4.Entities;
using Microsoft.IdentityModel.Tokens;
using SAP4.Services;
using SAP4.Repositories;


namespace SAP4.SapInvoiceService;
public class SapInvoiceService : BackgroundService
{

    private readonly ILogger<SapInvoiceService> _logger;
    private readonly ISapRepository _repository;
    private readonly ISapService _sapService;

    public SapInvoiceService(ILogger<SapInvoiceService> logger, ISapRepository invoiceRepository, ISapService sapService)
    {
        _logger = logger;
        _repository = invoiceRepository;
        _sapService = sapService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Invoice Processor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Fetch invoices from the repository
                var invoices = await _repository.GetAllInvokes();

                // Process each invoice
                foreach (var invoice in invoices)
                {
                    await ProcessInvoiceAsync(invoice);
                }

                _logger.LogInformation("Processed {Count} invoices.", invoices.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invoices.");
            }

            // Wait for the next execution (e.g., every 1 minute)
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public async Task ProcessInvoiceAsync(InvoiceSAP invoice)
    {
        _logger.LogInformation("Processing invoice {InvoiceId}.", invoice.Id);

        try
        {
            // Validate the invoice
            if (ValidateInvoice(invoice))
            {
                // Post the invoice to the accounting system
                await _sapService.AddAsync(invoice);

                _logger.LogInformation("Invoice {InvoiceId} posted to the system.", invoice.Id);
            }
            else
            {
                _logger.LogWarning("Invoice {InvoiceId} failed validation.", invoice.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing invoice {InvoiceId}.", invoice.Id);
        }
    }

    private bool ValidateInvoice(InvoiceSAP invoice)
    {
        // Add validation logic
        if (invoice == null)
        {
            _logger.LogWarning("Invoice is null.");
            return false;
        }

        if (invoice.Status == 0)
        {
            _logger.LogWarning("Invoice {InvoiceId} has an unknown Status: ${Status}.", invoice.Id, invoice.Status);
            return false;
        }

        return true;
    }
}




