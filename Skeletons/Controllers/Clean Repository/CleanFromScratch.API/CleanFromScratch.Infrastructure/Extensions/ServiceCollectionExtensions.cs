

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
            services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase(configuration.GetConnectionString("RestaurantDb")));
           
            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantsRepository, RestauranRepository>();
        }
    }
}
