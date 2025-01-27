﻿using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Entities;
using WMS.Interfaces;
using WMS.Repositories;

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
                    throw new InvalidOperationException($"Product not found");
                }

                if (stock.OperationType == Enums.OperationType.Load)
                {
                    existingProduct.Quantity -= item.Quantity;
                }
                else if (stock.OperationType == Enums.OperationType.Unload)
                {
                    if (existingProduct.Quantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient quantity for product with ID {item.Id}. Available quantity: {existingProduct.Quantity}");
                    }
                    existingProduct.Quantity += item.Quantity;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid operation type");
                }
                await _productRepository.UpdateProduct(existingProduct);
            }

            var baseUrl = "https://localhost:7046/api/Sap/wms/";
            var fullUrl = $"{baseUrl}{stock.InvoiceId}";
            FetchDataAsync(fullUrl);

            return stock;

        }

        public async Task<Stock> GetStockById(int id)
        {
            return await _stockRepository.GetStockById(id);
        }

        public async Task<string> FetchDataAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

/*
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
 */