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


namespace CleanFromScratch.API.Endpoints;

public class RestaurantsEndpoint : IMinimalEndpoint
{


    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var restaurantsGroup = routeBuilder.MapGroup("api/restaurants")
            .WithTags("Restaurants endpoints");

        restaurantsGroup.MapGet("/", async ([FromServices] IMediator mediator, [AsParameters] GetAllRestaurantsQuery query) =>
        {
            var restaurants = await mediator.Send(query);
            return Results.Ok(restaurants);
        })
        .AllowAnonymous()
        .Produces<IEnumerable<RestaurantDto>>(StatusCodes.Status200OK);

        restaurantsGroup.MapGet("/{id}", async ([FromServices] IMediator mediator, int id) =>
        {
            var restaurant = await mediator.Send(new GetRestaurantsByIdQuery(id));
            return Results.Ok(restaurant);
        })
        .RequireAuthorization()
        .Produces<RestaurantDto>(StatusCodes.Status200OK);

        restaurantsGroup.MapPost("/", [Authorize(Roles = UserRoles.Owner)] async ([FromServices] IMediator mediator, [FromBody] CreateRestaurantCommand command) =>
        {
            int id = await mediator.Send(command);
            return Results.Created($"/api/restaurants/{id}", null);
        })
        .RequireAuthorization();

        restaurantsGroup.MapPatch("/{id}", async ([FromServices] IMediator mediator, int id, [FromBody] UpdateRestaurantCommand command) =>
        {
            command.Id = id;
            await mediator.Send(command);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        restaurantsGroup.MapDelete("/{id}", async ([FromServices] IMediator mediator, int id) =>
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}