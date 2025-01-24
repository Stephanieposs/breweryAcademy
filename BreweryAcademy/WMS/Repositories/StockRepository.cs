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

        public async Task<Stock> UpdateQuantity(Stock stock)
        {
            _context.Entry(stock).State = EntityState.Modified;
            foreach (var product in stock.Products)
            {
                _context.Entry(product).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return stock;
        }
    }
}
