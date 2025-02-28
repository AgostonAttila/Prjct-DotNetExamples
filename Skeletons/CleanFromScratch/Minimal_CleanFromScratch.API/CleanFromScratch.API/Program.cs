using CleanFromScratch.API.Middleware;
using CleanFromScratch.Appplication.Extensions;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Infrastructure.Extensions;
using CleanFromScratch.Infrastructure.Seeders;
using CleanFromScratch.API.Extensions;
using Serilog;
using CleanFromScratch.API.Endpoints;


var builder = WebApplication.CreateBuilder(args);


builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrasturcture(builder.Configuration);



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

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("api/restaurants")
   .WithTags(" Restaurants endpoints")
   .MapRestaurantsEndpoints();

app.MapGroup("/api/restaurant")
    .WithTags("Dishes endpoints")
    .MapDishesEndpoints();

app.UseAuthorization();

app.Run();
