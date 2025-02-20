using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Appplication.Restaurants
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int> Create(Restaurant entity);
        Task Delete(Restaurant entity);
        Task SaveChanges();
    }
}