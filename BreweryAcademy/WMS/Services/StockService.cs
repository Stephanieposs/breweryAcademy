using WMS.Entities;
using WMS.Interfaces;
using WMS.Repositories;

namespace WMS.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;

        public StockService(IStockRepository stockRepository, IProductRepository productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            return await _stockRepository.GetAllStocks();
        }

        public async Task<Stock> UpdateQuantity(Stock stock)
        {
            foreach (var product in stock.Products)
            {
                var existingProduct = await _productRepository.GetProductById(product.Id);

                if (existingProduct == null)
                {
                    throw new InvalidOperationException($"Product not found");
                }

                if (stock.OperationType == Enums.OperationType.Load)
                {
                    existingProduct.Quantity += product.Quantity;
                }
                else if (stock.OperationType == Enums.OperationType.Unload)
                {
                    if (existingProduct.Quantity < product.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient quantity for product with ID {product.Id}. Available quantity: {existingProduct.Quantity}");
                    }
                    existingProduct.Quantity -= product.Quantity;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid operation type");
                }
                
            }
            await _stockRepository.UpdateQuantity(stock);
            return stock;
        }
    }
}
