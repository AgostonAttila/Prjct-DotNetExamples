using CleanFromScratch.Appplication.Extensions;
using CleanFromScratch.Infrastructure.Extensions;
using CleanFromScratch.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddApplication();
builder.Services.AddInfrasturcture(builder.Configuration);


var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
