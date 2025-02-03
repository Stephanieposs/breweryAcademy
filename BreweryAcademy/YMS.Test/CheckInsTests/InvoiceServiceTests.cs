using Moq;
using Xunit;
using System.Threading.Tasks;
using YMS.Services;
using YMS.Repositories;
using YMS.Entities;
using YMS.DTO;
using AutoMapper;
using FluentAssertions;
using YMS.Abstractions;
using YMS.DTO.Invoice.UpdateInvoice;
using YMS.Enums;
using FluentValidation;
using YMS.Exceptions;

namespace YMS.Test.CheckInsTests
{
	public class InvoiceServiceTests
	{
		private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly InvoiceService _invoiceService;

		public InvoiceServiceTests()
		{
			_invoiceRepositoryMock = new Mock<IInvoiceRepository>();
			_mapperMock = new Mock<IMapper>();
			_invoiceService = new InvoiceService(_invoiceRepositoryMock.Object, _mapperMock.Object);
		}

		[Fact (DisplayName ="Given valid status, update invoice then returns object")]
		public async Task InvoiceService_UpdateInvoice_ReturnsObject()
		{
			// Arrange
			var request = new UpdateInvoiceCommand
			{
				Status = "Inactive"
			};
			var invoiceId = 1;
			var invoice = new Invoice
			{
				CheckInId = 1,
				Id = 1,
				InvoiceId = 1,
				InvoiceStatus = Enums.InvoiceStatus.Inactive,
				InvoiceType = InvoiceType.Load,
				Items = new()
				{
					new InvoiceItem
					{
						InvoiceId = 1,
						InvoiceItemId = 1,
						ProductId = 1,
						Quantity = 10
					}
				}
			};

			_invoiceRepositoryMock.Setup(repo => repo.GetInvoiceByExternalId(invoiceId))
				.ReturnsAsync(invoice);

			_invoiceRepositoryMock.Setup(repo => repo.UpdateInvoiceStatus(It.IsAny<Invoice>()))
				.ReturnsAsync((Invoice inv) => inv);

			var expectedResult = new UpdateInvoiceResult
			{
				Id = 1,
				InvoiceId = 1,
				InvoiceStatus = "Active",
				InvoiceType = "Load",
				Items = new()
				{
					new InvoiceItemDto
					{
						ProductId = 1,
						Quantity = 10
					}
				}
			};

			_mapperMock.Setup(mapper => mapper.Map<UpdateInvoiceResult>(invoice))
				.Returns(expectedResult);

			// Act
			var result = await _invoiceService.UpdateInvoice(request, invoiceId);

			// Assert
			result.InvoiceStatus.Should().Be("Active");

			_invoiceRepositoryMock.Verify(repo => repo.UpdateInvoiceStatus(It.IsAny<Invoice>()), Times.Once);
			_invoiceRepositoryMock.Verify(repo => repo.GetInvoiceByExternalId(invoiceId), Times.Once);
		}

		[Fact(DisplayName ="Given invalid status, update invoice then throws exception")]
		public async Task InvoiceService_UpdateInvoice_ThrowsException()
		{
			var request = new UpdateInvoiceCommand
			{
				Status = "Inact1ve"
			};
			var invoiceId = 1;
			Func<Task> act = async() => await _invoiceService.UpdateInvoice(request, invoiceId);

			await act.Should().ThrowAsync<ValidationException>();
		}

		[Fact(DisplayName = "Given invalid invoice, update invoice then throws exception")]
		public async Task InvoiceService_NotExistentUpdateInvoice_ThrowsException()
		{
			var request = new UpdateInvoiceCommand
			{
				Status = "Inactive"
			};
			var invoiceId = 7777;
			_invoiceRepositoryMock.Setup(x => x.GetInvoiceByExternalId(invoiceId)).ReturnsAsync((Invoice?)null);



			Func<Task> act = async () => await _invoiceService.UpdateInvoice(request, invoiceId);

			await act.Should().ThrowAsync<InvoiceNotFoundException>();
		}
	}
}
