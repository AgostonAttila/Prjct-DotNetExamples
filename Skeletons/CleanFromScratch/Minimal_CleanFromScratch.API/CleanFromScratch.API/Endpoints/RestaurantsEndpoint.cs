using CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;
using CleanFromScratch.Application.Restaurants.Queries.GetRestaurantsById;
using CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace CleanFromScratch.API.Endpoints;

public static class RestaurantsEndpoint
{


    public static void MapRestaurantsEndpoints(this IEndpointRouteBuilder endpoints)
    {

       

        endpoints.MapGet("/", async (IMediator mediator, [AsParameters] GetAllRestaurantsQuery query) =>
        {
            var restaurants = await mediator.Send(query);
            return Results.Ok(restaurants);
        })
        .AllowAnonymous()
        .Produces<IEnumerable<RestaurantDto>>(StatusCodes.Status200OK);

        endpoints.MapGet("/{id}", async (IMediator mediator, int id) =>
        {
            var restaurant = await mediator.Send(new GetRestaurantsByIdQuery(id));
            return Results.Ok(restaurant);
        })
        .RequireAuthorization()
        .Produces<RestaurantDto>(StatusCodes.Status200OK);

        endpoints.MapPost("/", [Authorize(Roles = UserRoles.Owner)] async (IMediator mediator, CreateRestaurantCommand command) =>
        {
            int id = await mediator.Send(command);
            return Results.Created($"/api/restaurants/{id}", null);
        })
        .RequireAuthorization();

        endpoints.MapPatch("/{id}", async (IMediator mediator, int id, UpdateRestaurantCommand command) =>
        {
            command.Id = id;
            await mediator.Send(command);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{id}", async (IMediator mediator, int id) =>
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            return Results.NoContent();
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}