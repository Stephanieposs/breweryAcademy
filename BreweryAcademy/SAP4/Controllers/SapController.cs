using BuildingBlocks.Exceptions;
using MassTransit;
using MassTransit.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SAP4.Entities;
using SAP4.Extensions;
using SAP4.Services;
using System.ComponentModel.DataAnnotations;

namespace SAP4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SapController : Controller
{
    private readonly ISapService _sapService;

    public SapController(ISapService sapService)
    {
        _sapService = sapService;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetAll()
    {
        var invoices = await _sapService.GetAllAsync();
        return Ok(invoices);
    }

    [HttpGet("yms/{id}")]
    public async Task<ActionResult<InvoiceSAP>> GetByIdFromYms(int id)
    {
        var invoice = await _sapService.GetByIdAsync(id);
        if (invoice == null)
        {
            throw new NotFoundException("InvoiceId", id);
        }
        return Ok(invoice);
    }

    [HttpGet("wms/{id}")]
    public async Task<ActionResult<InvoiceSAP>> GetByIdFromWms(int id, IBus bus)
    {
        var invoice = await _sapService.GetByIdAsync(id);
        if (invoice == null)
        {
            throw new NotFoundException("InvoiceId", id);
        }

        invoice.Status = InvoiceStatus.Inactive;

        await _sapService.UpdateStatusAsync(invoice);

        var eventRequest = new RecordRequestedEvent(invoice.Id, invoice.Status);

        await bus.Publish(eventRequest); // passa para dentro do rabbitMQ

        return Ok(invoice);
    }

    [HttpPost]
    public async Task<ActionResult> Create(InvoiceSAP invoice) 
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState.ToString());
        }

        var added =  _sapService.AddAsync(invoice);


        return Ok(invoice);
    }

}
