using FakeItEasy;
using FluentAssertions;
using YMS.Abstractions;
using YMS.DTO;
using YMS.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using YMS.Entities;
using Moq;
using FluentValidation;
using BuildingBlocks.Exceptions;

namespace YMS.Test.CheckIns
{
	public class CheckInServiceTests
	{
		private readonly CheckInService _service;
		private readonly ICheckInRepository _repository;
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly Mock<HttpMessageHandler> _httpMessageHandler;

		public CheckInServiceTests()
		{
			// Fake Dependencies
			_repository = A.Fake<ICheckInRepository>();
			_mapper = A.Fake<IMapper>();
			_configuration = A.Fake<IConfiguration>();
			_httpMessageHandler = new Mock<HttpMessageHandler>();	
			_httpClient = A.Fake<HttpClient>();
			_configuration = A.Fake<IConfiguration>();
			_service = A.Fake<CheckInService>(opt => opt.CallsBaseMethods());

			// Fake HTTP Message Handler for HttpClient

			// Initialize service with fakes
		}

		[Fact (DisplayName = "Given valid object, when adding checkin, then returns object")]
		public async Task CheckInRepository_CreateCheckIn_ReturnsObject()
		{
			//arrange
			var request = new CreateCheckInBody
			{
				Invoice = new InvoiceDto
				{
					InvoiceId = 123,
					InvoiceType = "Load", // Example invoice type
					Items = new List<InvoiceItemDto>
				{
					new InvoiceItemDto { ProductId = 456, Quantity = 10 },
					new InvoiceItemDto { ProductId = 789, Quantity = 5 },
					new InvoiceItemDto { ProductId = 101, Quantity = 15 }
				}
				},
				DriverDocument = "1234567890", 
				TruckPlate = "ABC-1234"
			};

			var checkIn = new CheckIn();
			var checkInResponse = new CreateCheckInResponse();

			//act

			//validate SAP
			A.CallTo(_service).Where(x => x.Method.Name == "ValidateSapInvoice").WithReturnType<Task<bool>>().Returns(Task.FromResult(true));

			//send to WMS
			A.CallTo(_service).Where(x => x.Method.Name == "SendWMSStockExchange").WithReturnType<Task<bool>>().Returns(Task.FromResult(true));

			//mock repository behaviour

			A.CallTo(() => _mapper.Map<CheckIn>(request)).Returns(checkIn);
			A.CallTo(() => _repository.CreateCheckIn(checkIn)).Returns(checkIn);
			A.CallTo(()=> _mapper.Map<CreateCheckInResponse>(request)).Returns(checkInResponse);

			//implement behaviour in service

			var result = await _service.CreateCheckIn(request);

			result.Should().NotBeNull();
			result.Should().BeAssignableTo<CreateCheckInResponse>();
			result.Should().BeEquivalentTo(checkInResponse);


		}

		[Fact(DisplayName ="Given invalid object, when adding checkin, then throws exception")]
		public async Task CheckInRepository_CreateCheckIn_ThrowsException()
		{
			//arrange
			var request = new CreateCheckInBody
			{
				Invoice = new InvoiceDto
				{
					InvoiceId = 1235,
					InvoiceType = "Load12", // Example invoice type
					Items = new List<InvoiceItemDto>()
				},
				DriverDocument = "1234567890",
				TruckPlate = "ABC-1234"
			};

			//act
			Func<Task> act = async () => await _service.CreateCheckIn(request);

			//assert
			await act.Should().ThrowAsync<ValidationException>();
		}

		[Fact(DisplayName = "Given invalid invoice, when adding checkin, then throws exception")]
		public async Task CheckInService_CreateCheckIn_ThrowsExceptionIfFailsToValidateInvoice()
		{
			//arrange
			var request = new CreateCheckInBody
			{
				Invoice = new InvoiceDto
				{
					InvoiceId = 123,
					InvoiceType = "Load", // Example invoice type
					Items = new List<InvoiceItemDto>
				{
					new InvoiceItemDto { ProductId = 456, Quantity = 10 },
					new InvoiceItemDto { ProductId = 789, Quantity = 5 },
					new InvoiceItemDto { ProductId = 101, Quantity = 15 }
				}
				},
				DriverDocument = "1234567890",
				TruckPlate = "ABC-1234"
			};

			//act
			A.CallTo(_service).Where(x => x.Method.Name == "ValidateSapInvoice").WithReturnType<Task<bool>>().Returns(Task.FromResult(false));
			Func<Task> act = async () => await _service.CreateCheckIn(request);

			//assert
			await act.Should().ThrowAsync<BadRequestException>();
		}
	}
}
