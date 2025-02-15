
namespace OFinance.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        Task<int> SaveChangesAsync();
    }
}
