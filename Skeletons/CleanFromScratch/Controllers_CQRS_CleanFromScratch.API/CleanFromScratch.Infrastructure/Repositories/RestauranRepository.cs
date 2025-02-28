using System.Linq.Expressions;
using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using CleanFromScratch.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task Delete(Restaurant restaurant)
        {
            dbContext.Restaurants.Remove(restaurant);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.ToListAsync();
            return restaurants;
        }

        public async Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(string? searchPhrase, int pageSize,int pageNumber, string? sortBy, SortDirection sortDirection)
        {
            var searchPhraseToLower = searchPhrase?.ToLower() ?? string.Empty;

            var baseQuery = dbContext
                .Restaurants
                .Where(r => searchPhraseToLower == null || (r.Name.ToLower().Contains(searchPhraseToLower) ||
                                                           r.Description.ToLower().Contains(searchPhraseToLower)));


            var totalItems = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name),r=>r.Name },
                    { nameof(Restaurant.Description),r=>r.Description },
                    { nameof(Restaurant.Category),r=>r.Category },
                };

                var selectedColumn = columnsSelector[sortBy];

                baseQuery = sortDirection switch
                {
                    SortDirection.Ascending => baseQuery.OrderBy(selectedColumn),
                    SortDirection.Descending => baseQuery.OrderByDescending(selectedColumn),
                    _ => baseQuery
                };
            }

            var restaurants = await baseQuery
                .Skip(pageSize*(pageNumber-1))
                .Take(pageSize)
                .ToListAsync();

            return (restaurants,totalItems);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(p => p.Dishes)
                .FirstOrDefaultAsync(p => p.Id == id);
            return restaurant;
        }

        public Task SaveChanges()
        => dbContext.SaveChangesAsync();
    }
}
