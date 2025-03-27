using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Appplication.Restaurants
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantDto>> GetAll();
        Task<RestaurantDto?> GetById(int id);
        Task<int> Create(CreateRestaurantDto dto);
    }
}