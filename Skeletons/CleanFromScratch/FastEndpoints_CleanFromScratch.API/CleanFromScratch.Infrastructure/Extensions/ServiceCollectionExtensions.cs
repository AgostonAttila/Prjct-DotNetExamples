

using CleanFromScratch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CleanFromScratch.Infrastructure.Seeders;
using CleanFromScratch.Domain.Repositories;
using CleanFromScratch.Infrastructure.Repositories;
using CleanFromScratch.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Restaurants.Infrastructure.Authorization;
using CleanFromScratch.Infrastructure.Authorization.Requirements;
using CleanFromScratch.Infrastructure.Authorization;
using CleanFromScratch.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;

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
           
            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantDbContext>();

            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantsRepository, RestauranRepository>();
            services.AddScoped<IDishesRepository, DishesRepository>();
       
            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.HasNationality,
             builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"))
                .AddPolicy(PolicyNames.AtLeast20,
             builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
                .AddPolicy(PolicyNames.CreatedAtleast2Restaurants,
             builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();
            services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        }
    }
}
