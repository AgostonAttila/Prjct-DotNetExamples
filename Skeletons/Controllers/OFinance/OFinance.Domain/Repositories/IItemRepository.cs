using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OFinance.Domain.Entities;

namespace OFinance.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(Guid id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item> AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Guid id);
    }
}
