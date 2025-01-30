using BuildingBlocks.Exceptions;
using WMS.Entities;
using WMS.Interfaces;

namespace WMS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            if (product.Quantity < 0)
            {
                throw new BadRequestException("Product quantity cannot be negative.");
            }

            return await _productRepository.CreateProduct(product);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            if (product.Quantity < 0)
            {
                throw new BadRequestException("Product quantity cannot be negative.");
            }

            return await _productRepository.UpdateProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }
    }
}
