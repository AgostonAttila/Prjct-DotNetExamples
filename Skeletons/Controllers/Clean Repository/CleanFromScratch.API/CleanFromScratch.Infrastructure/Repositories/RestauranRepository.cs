using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using CleanFromScratch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanFromScratch.Infrastructure.Repositories
{
    internal class RestauranRepository(RestaurantDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> Create(Restaurant entity)
        {
            dbContext.Restaurants.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.ToListAsync();
            return restaurants;
        }

        public  async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(p => p.Dishes)
                .FirstOrDefaultAsync(p=>p.Id == id);
            return restaurant;
        }
    }
}
