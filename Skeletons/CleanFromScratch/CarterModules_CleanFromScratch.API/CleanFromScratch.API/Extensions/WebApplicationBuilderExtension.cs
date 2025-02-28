using Carter;
using CleanFromScratch.API.Middleware;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CleanFromScratch.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddCarter();

        //TODO Scalar
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                    },
                    []
                }

            });
        });


        builder.Services.AddEndpointsApiExplorer();

        //TODO ProblemDetails
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

       
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
        //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        //.WriteTo.File("Logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day,rollOnFileSizeLimit: true)
        //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] |{SourceContext} | {NewLine}{Message:lj}{Exception}"));
    }

}
