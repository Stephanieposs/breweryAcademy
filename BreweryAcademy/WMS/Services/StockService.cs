using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Entities;
using WMS.Interfaces;
using WMS.Repositories;
using System.Text.Json;


namespace WMS.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;
        private readonly HttpClient _httpClient;

        public StockService(IStockRepository stockRepository, IProductRepository productRepository, HttpClient httpClient)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            return await _stockRepository.GetAllStocks();
        }

        public async Task<Stock> CreateStock(Stock stock)
        {
            
				var newStock = await _stockRepository.CreateStock(stock);

                foreach (var item in stock.Products)
                {
                    var existingProduct = await _productRepository.GetProductById(item.Id);

                    if (existingProduct == null)
                    {
                        throw new NotFoundException("Product", item.Id);
                    }
                    if (item.Quantity < 0)
                    {
                        throw new InvalidOperationException("Quantity cannot be negative");
                    }

                    if (stock.OperationType == Enums.OperationType.Load)
                    {
                        if (existingProduct.Quantity < item.Quantity)
                        {
                            throw new InvalidOperationException($"Insufficient quantity for product with ID {item.Id}. Available quantity: {existingProduct.Quantity}");
                        }
                        existingProduct.Quantity -= item.Quantity;
                    }
                    else if (stock.OperationType == Enums.OperationType.Unload)
                    {
                        existingProduct.Quantity += item.Quantity;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid operation type");
                    }
                    await _productRepository.UpdateProduct(existingProduct);
                }
				    var baseUrl = "https://localhost:7046/api/Sap/wms/";
				    var fullUrl = $"{baseUrl}{stock.InvoiceId}";
				    var inactivateInvoice = await FetchDataAsync(fullUrl);
				   
                    if (!inactivateInvoice)
				    {
					    throw new BadRequestException("Impossible to validate Invoice");
				    }

				return stock;
        }

        public async Task<Stock> GetStockById(int id)
        {
            return await _stockRepository.GetStockById(id);
        }

        public virtual async Task<bool> FetchDataAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            return response.IsSuccessStatusCode;
        }
    }
}

