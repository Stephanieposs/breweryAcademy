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

namespace WmsTest
{
    public class StockServiceTest
    {
        private readonly Mock<IStockRepository> _mockStockRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;
        private readonly IStockService _stockService;

        public StockServiceTest()
        {
            _mockStockRepository = new Mock<IStockRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockHttpClient = new Mock<IHttpClientWrapper>();
            _stockService = new StockService(_mockStockRepository.Object, _mockProductRepository.Object, _mockHttpClient.Object);
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

            _mockProductRepository.Setup(repo => repo.GetProductById(1)).ReturnsAsync(existingProduct);
            _mockStockRepository.Setup(repo => repo.CreateStock(stock)).ReturnsAsync(stock);

            var ymsStockData = "[{\"Id\": 1, \"Products\": [{\"Id\": 1, \"Quantity\": 5}]}]";
            _mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(ymsStockData)
            });

            //Act
            var result = await _stockService.CreateStock(stock);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(stock.InvoiceId, result.InvoiceId);
            Assert.Equal(OperationType.Load, result.OperationType);

            _mockProductRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
            _mockStockRepository.Verify(repo => repo.CreateStock(It.IsAny<Stock>()), Times.Once);
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

            _mockProductRepository.Setup(repo => repo.GetProductById(1)).ReturnsAsync(existingProduct);
            _mockStockRepository.Setup(repo => repo.CreateStock(stock)).ReturnsAsync(stock);

            var ymsStockData = "[{\"Id\": 1, \"Products\": [{\"Id\": 1, \"Quantity\": 5}]}]";
            _mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(ymsStockData)
            });

            //Act/Assert
            await Assert.ThrowsAsync<InternalServerErrorException>(() => _stockService.CreateStock(stock));
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

            _mockProductRepository.Setup(repo => repo.GetProductById(1)).ReturnsAsync(new Product { Id = 1, Quantity = 10 });
            _mockStockRepository.Setup(repo => repo.CreateStock(stock)).ReturnsAsync(stock);

            //simulate a failure in the external API
            _mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException("API request failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InternalServerErrorException>(() => _stockService.CreateStock(stock));
            Assert.Equal("An error occurred while processing the stock", exception.Message);
        }


    }
}
