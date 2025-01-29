using Microsoft.Extensions.Logging;
using SAP4.Entities;
using SAP4.SapInvoiceService;
using SAP4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Xunit;
using SAP4.Repositories;


namespace SapTest;

public class InvoiceServiceTest
{

    private readonly Mock<ILogger<SapInvoiceService>> _loggerMock;
    private readonly Mock<ISapRepository> _sapRepositoryMock;
    private readonly Mock<ISapService> _sapServiceMock;
    private readonly SapInvoiceService _service;

    public InvoiceServiceTest()
    {
        _loggerMock = new Mock<ILogger<SapInvoiceService>>();
        _sapRepositoryMock = new Mock<ISapRepository>();
        _sapServiceMock = new Mock<ISapService>();
        _service = new SapInvoiceService(_loggerMock.Object, _sapRepositoryMock.Object, _sapServiceMock.Object);
    }

    [Fact]
    public async Task ProcessInvoiceAsync_ValidInvoice_PostsInvoice()
    {
        // Arrange
        var invoice = new InvoiceSAP { Id = 1, Date = DateTime.Now, Status = InvoiceStatus.Active };
        _sapRepositoryMock.Setup(r => r.GetAllInvokes()).ReturnsAsync(new List<InvoiceSAP> { invoice });

        // Act
        await _service.ProcessInvoiceAsync(invoice);

        // Assert
        _sapServiceMock.Verify(s => s.AddAsync(invoice), Times.Once);
    }

}

