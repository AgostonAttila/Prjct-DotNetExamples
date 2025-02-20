

using CleanFromScratch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CleanFromScratch.Infrastructure.Seeders;
using CleanFromScratch.Domain.Repositories;
using CleanFromScratch.Infrastructure.Repositories;

namespace CleanFromScratch.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrasturcture(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RestaurantDb");
            services.AddDbContext<RestaurantDbContext>(options => 
                     options.UseInMemoryDatabase(connectionString)
                     .EnableSensitiveDataLogging());
           
            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantsRepository, RestauranRepository>();
            services.AddScoped<IDishesRepository, DishesRepository>();
        }
    }
}
