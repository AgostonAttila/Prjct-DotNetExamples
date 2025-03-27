using Carter;
using CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;
using CleanFromScratch.Application.Restaurants.Queries.GetRestaurantsById;
using CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Endpoints
{
    public class RestaurantsModule : CarterModule
    {
        public RestaurantsModule()
      : base("/api/restaurants")
        {
            WithTags("Restaurants");
            IncludeInOpenApi();  
            //this.RequireAuthorization();
        }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/", async ([FromServices] IMediator mediator, [AsParameters] GetAllRestaurantsQuery query) =>
            {
                var restaurants = await mediator.Send(query);
                return Results.Ok(restaurants);
            })
            .AllowAnonymous()
            .Produces<IEnumerable<RestaurantDto>>(StatusCodes.Status200OK);

            app.MapGet("/{id}", async ([FromServices] IMediator mediator, int id) =>
            {
                var restaurant = await mediator.Send(new GetRestaurantsByIdQuery(id));
                return Results.Ok(restaurant);
            })
            .RequireAuthorization()
            .Produces<RestaurantDto>(StatusCodes.Status200OK);

            app.MapPost("/", [Authorize(Roles = UserRoles.Owner)] async ([FromServices] IMediator mediator, [FromBody] CreateRestaurantCommand command) =>
            {
                int id = await mediator.Send(command);
                return Results.Created($"/api/restaurants/{id}", null);
            })
            .RequireAuthorization();

            app.MapPatch("/{id}", async ([FromServices] IMediator mediator, int id,[FromBody] UpdateRestaurantCommand command) =>
            {
                command.Id = id;
                await mediator.Send(command);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("/{id}", async ([FromServices] IMediator mediator, int id) =>
            {
                await mediator.Send(new DeleteRestaurantCommand(id));
                return Results.NoContent();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
