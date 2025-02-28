using CleanFromScratch.Application.Dishes.Commands.CreateDish;
using CleanFromScratch.Application.Dishes.Commands.DeleteDishes;
using CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;
using CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;
using CleanFromScratch.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CleanFromScratch.API.Endpoints
{
    public  class DishesEndpoints : IMinimalEndpoint
    {
        public  void MapRoutes( IEndpointRouteBuilder routeBuilder)
        {
            var restaurantsGroup = routeBuilder.MapGroup("/api/restaurant")
                .WithTags("Dishes endpoints");


            restaurantsGroup.MapPost("/{restaurantId}/dishes", async (int restaurantId, [FromBody] CreateDishCommand command,[FromServices] IMediator mediator) =>
            {
                command.RestaurantId = restaurantId;
                var dishId = await mediator.Send(command);
                return Results.Created($"/api/restaurant/{restaurantId}/dishes/{dishId}", null);
            });

            restaurantsGroup.MapGet("/{restaurantId}/dishes", async (int restaurantId, [FromServices] IMediator mediator) =>
            {
                var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
                return Results.Ok(dishes);
            });

            restaurantsGroup.MapGet("/{restaurantId}/dishes/{dishId}", async (int restaurantId, int dishId, [FromServices] IMediator mediator) =>
            {
                var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
                return Results.Ok(dish);
            });

            restaurantsGroup.MapDelete("/{restaurantId}/dishes", async (int restaurantId, [FromServices] IMediator mediator) =>
            {
                await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
                return Results.NoContent();
            });
        }
    }
}
