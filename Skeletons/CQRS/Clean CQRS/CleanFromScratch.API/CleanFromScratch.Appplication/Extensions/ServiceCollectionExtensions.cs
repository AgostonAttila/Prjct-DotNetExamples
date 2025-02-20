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

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssembly(assembly)
                    .AddFluentValidationAutoValidation();
        }
    }
}
