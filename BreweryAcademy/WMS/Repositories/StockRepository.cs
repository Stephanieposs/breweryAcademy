using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Entities;
using WMS.Interfaces;

namespace WMS.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly DefaultContext _context;

        public StockRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            return await _context.Stocks
                .Include(s => s.Products)
                .ToListAsync();
        }

        public async Task<Stock> CreateStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> GetStockById(int id)
        {
            return await _context.Stocks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
