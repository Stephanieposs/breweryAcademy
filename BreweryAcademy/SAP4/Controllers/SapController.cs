
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SAP4.Entities;
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
        if (invoices == null)
        {
            return NotFound();
        }
        return Ok(invoices);
    }

    [HttpGet("yms/{id}")]
    public async Task<ActionResult<InvoiceSAP>> GetByIdFromYms(int id)
    {
        var invoice = await _sapService.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }
        return Ok(invoice);
    }

    [HttpGet("wms/{id}")]
    public async Task<ActionResult<InvoiceSAP>> GetByIdFromWms(int id)
    {
        var invoice = await _sapService.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        await _sapService.UpdateStatusAsync(invoice);

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> Create(InvoiceSAP invoice)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _sapService.AddAsync(invoice);
        return Ok(invoice);
    }

}
