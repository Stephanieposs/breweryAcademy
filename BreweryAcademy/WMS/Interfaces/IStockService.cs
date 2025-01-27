using WMS.Entities;

namespace WMS.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocks();
        Task<Stock> CreateStock(Stock stock);
        Task<Stock> GetStockById(int id);
    }
}
