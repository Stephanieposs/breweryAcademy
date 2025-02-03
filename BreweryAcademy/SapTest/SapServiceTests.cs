using BuildingBlocks.Exceptions;
using Moq;
using SAP4.Entities;
using SAP4.Repositories;
using SAP4.Services;
using Xunit;
using System;
using System.Threading.Tasks;

namespace SapTest;
public class SapServiceTests
{
    private readonly Mock<ISapRepository> _mockRepo;
    private readonly SapService _service;
    public SapServiceTests() //Mock<ISapRepository> mockRepo, SapService service
    {
        _mockRepo = new Mock<ISapRepository>(); // Initialize the mock
        _service = new SapService(_mockRepo.Object); // Create the service, injecting the mock
    }

    [Fact]
    public async Task AddAsync_NullInvoice_ThrowsBadRequestException()
    {
        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _service.AddAsync(null));

        // Assert
        Assert.NotNull(exception); 
    }

    [Fact]
    public async Task AddAsync_ValidInvoice_CallsRepositoryAddInvoice()
    {
        // Arrange
        var invoice = new InvoiceSAP { Id = 1, Date = DateTime.UtcNow, Status = InvoiceStatus.Active };

        // Act
        await _service.AddAsync(invoice);

        // Assert
        _mockRepo.Verify(repo => repo.AddInvoice(invoice), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllInvoices()
    {
        // Arrange
        var invoices = new List<InvoiceSAP> {
            new InvoiceSAP{Id=1, Status=InvoiceStatus.Active, Date = DateTime.UtcNow }, 
            new InvoiceSAP{Id=2, Status=InvoiceStatus.Active, Date = DateTime.UtcNow } };
        _mockRepo.Setup(repo => repo.GetAllInvoices()).ReturnsAsync(invoices);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(invoices.Count, result.Count());
    }

    [Theory]  // Parameterized tests
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_InvalidId_ThrowsBadRequestException(int invalidId)
    {
        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _service.GetByIdAsync(invalidId));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsInvoice()
    {
        // Arrange
        var invoice = new InvoiceSAP { Id = 1 };
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(invoice);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(invoice.Id, result.Id);
    }

    [Fact]
    public async Task UpdateStatusAsync_NullInvoice_ThrowsBadRequestException()
    {
        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _service.UpdateStatusAsync(null));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateStatusAsync_InvalidId_ThrowsBadRequestException()
    {
        // Arrange & Act & Assert
        var invoice = new InvoiceSAP();
        await Assert.ThrowsAsync<BadRequestException>(async () => await _service.UpdateStatusAsync(invoice));
    }

    [Fact]
    public async Task UpdateStatusAsync_InvoiceNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var invoice = new InvoiceSAP { Id = 1 };
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync((InvoiceSAP)null); 

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _service.UpdateStatusAsync(invoice));
    }

    [Fact]
    public async Task UpdateStatusAsync_ValidInvoice_CallsRepositoryUpdateInvoiceStatus()
    {
        // Arrange
        var invoice = new InvoiceSAP { Id = 1 };
        var existingInvoice = new InvoiceSAP { Id = 1 }; // Existing invoice from the repo
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(existingInvoice);

        // Act
        await _service.UpdateStatusAsync(invoice);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateInvoiceStatus(existingInvoice), Times.Once); // Verify with existingInvoice
    }

}
