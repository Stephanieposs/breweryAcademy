using BuildingBlocks.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Entities;
using WMS.Interfaces;
using WMS.Services;

namespace WmsTest
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductService _productService;

        public ProductServiceTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Quantity = 10
            };
            _mockProductRepository.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync(product);

            //Act
            var result = await _productService.GetProductById(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task GetProductById_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            //Arrange
            _mockProductRepository.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync((Product)null);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductById(1));
        }

        [Fact]
        public async Task CreateProduct_ShouldThrowBadRequestException_WhenQuantityIsNegative()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Invalid Product",
                Quantity = -5
            };

            //Act/Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _productService.CreateProduct(product));
            _mockProductRepository.Verify(repo => repo.CreateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProduct_ShouldThrowBadRequestException_WhenQuantityIsNegative()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Product", Quantity = -5 };

            // Act/Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _productService.UpdateProduct(product));

            Assert.Equal("Product quantity cannot be negative.", exception.Message);
            _mockProductRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }


    }
}
