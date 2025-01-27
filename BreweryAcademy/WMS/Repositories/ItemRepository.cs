using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Entities;
using WMS.Interfaces;

namespace WMS.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DefaultContext _context;

        public ItemRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Item> CreateItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
