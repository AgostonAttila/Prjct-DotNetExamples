using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OFinance.Domain.Repositories;
using OFinance.Inrastructure.Data;

namespace OFinance.Inrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ItemDbContext _context;
        private IItemRepository? _itemRepository;

        public UnitOfWork(ItemDbContext context)
        {
            _context = context;
        }

        public IItemRepository Items => _itemRepository ??= new ItemRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
