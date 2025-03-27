using Carter;
using CleanFromScratch.Application.Dishes.Commands.CreateDish;
using CleanFromScratch.Application.Dishes.Commands.DeleteDishes;
using CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;
using CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Endpoints
{
    public class DishesModule : CarterModule
    {
        public DishesModule()
        : base("/api/restaurant")
        {
            WithTags("Dishes");
            IncludeInOpenApi();
        }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/{restaurantId}/dishes", async (int restaurantId, [FromBody] CreateDishCommand command, [FromServices] IMediator mediator) =>
            {
                command.RestaurantId = restaurantId;
                var dishId = await mediator.Send(command);
                return Results.Created($"/api/restaurant/{restaurantId}/dishes/{dishId}", null);
            });

            app.MapGet("/{restaurantId}/dishes", async (int restaurantId, [FromServices] IMediator mediator) =>
            {
                var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
                return Results.Ok(dishes);
            });

            app.MapGet("/{restaurantId}/dishes/{dishId}", async (int restaurantId, int dishId, [FromServices] IMediator mediator) =>
            {
                var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
                return Results.Ok(dish);
            });

            app.MapDelete("/{restaurantId}/dishes", async (int restaurantId, [FromServices] IMediator mediator) =>
            {
                await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
                return Results.NoContent();
            });
        }
    }
}
