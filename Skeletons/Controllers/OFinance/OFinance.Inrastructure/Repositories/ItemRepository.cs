using Microsoft.EntityFrameworkCore;
using OFinance.Domain.Entities;
using OFinance.Domain.Repositories;
using OFinance.Inrastructure.Data;

namespace OFinance.Inrastructure.Repositories
{  
    public class ItemRepository : IItemRepository
    {
        private readonly ItemDbContext _context;

        public ItemRepository(ItemDbContext context)
        {
            _context = context;
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> AddAsync(Item item)
        {
            var result = await _context.Items.AddAsync(item);
            return result.Entity;
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
        }

    }
}
