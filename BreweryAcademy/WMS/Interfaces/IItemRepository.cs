using WMS.Entities;

namespace WMS.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> CreateItem(Item item);
        Task<Item> GetItemById(int id);
    }
}
