using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WMS.Entities;
using WMS.Interfaces;
using WMS.Services;
using WMS.Enums;
using BuildingBlocks.Exceptions;
using FakeItEasy;
using FluentAssertions;

namespace WmsTest
{
    public class StockServiceTest
    {
        private readonly IStockRepository _mockStockRepository;
        private readonly IProductRepository _mockProductRepository;
        private readonly IHttpClientWrapper _mockHttpClient;
        private readonly StockService _stockService;

        public StockServiceTest()
        {
            _mockStockRepository = A.Fake<IStockRepository>();
            _mockProductRepository = A.Fake<IProductRepository>();
            _mockHttpClient = A.Fake<IHttpClientWrapper>();
            _stockService = A.Fake<StockService>();
        }

        [Fact]
        public async Task CreateStock_ShouldCreateStock_WhenDataIsValid()
        {
            //Arrange
            var stock = new Stock
            {
                InvoiceId = 12345,
                OperationType = OperationType.Load,
                Products = new List<Item>
                {
                    new Item {
                        InternalId = 1,
                        Id = 1,
                        Quantity = 5
                    }
                }
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Product",
                Quantity = 10
            };
			A.CallTo(() => _mockStockRepository.CreateStock(stock)).Returns(stock);
			A.CallTo(() => _mockProductRepository.GetProductById(existingProduct.Id)).Returns(existingProduct);
			A.CallTo(() => _mockProductRepository.UpdateProduct(existingProduct)).Returns(existingProduct);

			A.CallTo(_stockService).Where(x => x.Method.Name == "FetchDataAsync").WithReturnType<Task<bool>>().Returns(Task.FromResult(true));

			//act



			var result = await _stockService.CreateStock(stock);

            //Assert
            result.Should().BeAssignableTo<Stock>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateStock_ShouldThrowInvalidOperationException_WhenInsufficientQuantity()
        {
            //Arrange
            var stock = new Stock
            {
                InvoiceId = 12345,
                OperationType = OperationType.Load,
                Products = new List<Item>
                {
                    new Item {
                        InternalId = 1,
                        Id = 1,
                        Quantity = 15
                    }
                }
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Product",
                Quantity = 10
            };

			A.CallTo(() => _mockStockRepository.CreateStock(A<Stock>.Ignored)).Returns(stock);
			A.CallTo(() => _mockProductRepository.GetProductById(A<int>.Ignored)).Returns(existingProduct);
		    
            //act
			Func<Task> act = async() => await _stockService.CreateStock(stock);

			//Assert
			await act.Should().ThrowAsync<InvalidOperationException>();
		}

        [Fact]
        public async Task CreateStock_ShouldThrowInternalServerErrorException_WhenApiFails()
        {
            //Arrange
            var stock = new Stock
            {
                InvoiceId = 12345,
                OperationType = OperationType.Load,
                Products = new List<Item>
                {
                    new Item {
                        InternalId = 1,
                        Id = 1,
                        Quantity = 7
                    }
                }
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Product",
                Quantity = 10
            };

			A.CallTo(() => _mockStockRepository.CreateStock(stock)).Returns(stock);
			A.CallTo(() => _mockProductRepository.GetProductById(existingProduct.Id)).Returns(existingProduct);
			A.CallTo(() => _mockProductRepository.UpdateProduct(existingProduct)).Returns(existingProduct);

			A.CallTo(() => _stockService.FetchDataAsync(A<string>.Ignored)).Returns(Task.FromResult(false));

            //act
			Func<Task> act = async () => await _stockService.CreateStock(stock);

			//Assert
			await act.Should().ThrowAsync<InternalServerErrorException>();
		}

        
    }
}
