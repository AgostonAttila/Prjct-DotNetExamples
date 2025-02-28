using CleanFromScratch.Application.Dishes.Commands.CreateDish;
using CleanFromScratch.Application.Dishes.Commands.DeleteDishes;
using CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;
using CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;
using CleanFromScratch.Domain.Entities;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanFromScratch.API.Endpoints
{
    public static class DishesEndpoints
    {
        public static void MapDishesEndpoints(this IEndpointRouteBuilder endpoints)
        {
  

            endpoints.MapPost("/{restaurantId}/dishes", async (int restaurantId, CreateDishCommand command, IMediator mediator) =>
            {
                command.RestaurantId = restaurantId;
                var dishId = await mediator.Send(command);
                return Results.Created($"/api/restaurant/{restaurantId}/dishes/{dishId}", null);
            });

            endpoints.MapGet("/{restaurantId}/dishes", async (int restaurantId, IMediator mediator) =>
            {
                var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
                return Results.Ok(dishes);
            });

            endpoints.MapGet("/{restaurantId}/dishes/{dishId}", async (int restaurantId, int dishId, IMediator mediator) =>
            {
                var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
                return Results.Ok(dish);
            });

            endpoints.MapDelete("/{restaurantId}/dishes", async (int restaurantId, IMediator mediator) =>
            {
                await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
                return Results.NoContent();
            });
        }
    }
}
