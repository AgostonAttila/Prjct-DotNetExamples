using CleanFromScratch.API.Middleware;
using CleanFromScratch.Appplication.Extensions;
using CleanFromScratch.Infrastructure.Extensions;
using CleanFromScratch.Infrastructure.Seeders;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//TODO Scalar
builder.Services.AddSwaggerGen();

//TODO ProblemDetails
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

builder.Services.AddApplication();
builder.Services.AddInfrasturcture(builder.Configuration);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
//.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
//.WriteTo.File("Logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day,rollOnFileSizeLimit: true)
//.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] |{SourceContext} | {NewLine}{Message:lj}{Exception}"));

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
