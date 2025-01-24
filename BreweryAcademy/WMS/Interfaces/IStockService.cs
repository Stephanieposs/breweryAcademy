using WMS.Entities;

namespace WMS.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocks();
        Task<Stock> UpdateQuantity(Stock stock);
    }
}
