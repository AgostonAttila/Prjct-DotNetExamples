using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int> Create(Restaurant entity);
        Task Delete(Restaurant restaurant);
        Task SaveChanges();
    }
}
