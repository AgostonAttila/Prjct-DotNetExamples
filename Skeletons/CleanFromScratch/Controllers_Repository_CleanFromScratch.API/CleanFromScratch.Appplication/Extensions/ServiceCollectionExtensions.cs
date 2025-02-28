using CleanFromScratch.Appplication.Restaurants;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanFromScratch.Appplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddScoped<IRestaurantService, RestaurantService>();

            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssembly(assembly)
                    .AddFluentValidationAutoValidation();
        }
    }
}
