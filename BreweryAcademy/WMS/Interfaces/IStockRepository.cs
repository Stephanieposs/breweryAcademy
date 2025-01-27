using WMS.Entities;

namespace WMS.Interfaces
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllStocks();
        Task<Stock> UpdateQuantity(Stock stock);
    }
}
