using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Infrastructure.Persistence;

namespace CleanFromScratch.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {

            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = [
                new()
                {
                    Name = "McDonalds",
                    Description = "Fast food",
                    Category = "Fast food",
                    HasDelivery = true,
                    ContactEmail = "",
                    ContactNumber = "",
                    Address = new Address
                    {
                        City = "New York",
                        Street = "5th Avenue",
                        PostalCode = "10001"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Big Mac",
                            Description = "Tasty",
                            Price = 5.99m
                        },
                        new Dish
                        {
                            Name = "McNuggets",
                            Description = "Tasty",
                            Price = 4.99m
                        }
                    }
                },
                 new()
                {
                    Name = "McDonalds",
                    Description = "Fast food",
                    Category = "Fast food",
                    HasDelivery = true,
                    ContactEmail = "",
                    ContactNumber = "",
                    Address = new Address
                    {
                        City = "New York",
                        Street = "5th Avenue",
                        PostalCode = "10001"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Big Mac",
                            Description = "Tasty",
                            Price = 5.99m
                        },
                        new Dish
                        {
                            Name = "McNuggets",
                            Description = "Tasty",
                            Price = 4.99m
                        }
                    }
                }
                 ];

            return restaurants;
        }
    }
}
