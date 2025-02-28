using System.Net;
using CleanFromScratch.Application.Common;
using CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;
using CleanFromScratch.Application.Restaurants.Queries.GetRestaurantsById;
using CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Constants;
using FastEndpoints;
using MediatR;

namespace CleanFromScratch.API.Endpoints;

public class GetAllRestaurantsEndpoint(IMediator _mediator) : EndpointWithoutRequest<PagedResult<RestaurantDto>>
{  
  
    public override void Configure()
    {
        Get("api/restaurants");        
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var restaurants = await _mediator.Send(new GetAllRestaurantsQuery());
        await SendOkAsync(restaurants, ct);
    }
}

public class GetRestaurantByIdEndpoint(IMediator _mediator) : EndpointWithoutRequest<RestaurantDto>
{
    public override void Configure()
    {
        Get("api/restaurants/{id}");
        Roles(UserRoles.Owner,UserRoles.Admin,UserRoles.User);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var restaurant = await _mediator.Send(new GetRestaurantsByIdQuery(id));
        await SendOkAsync(restaurant, ct);
    }
}

public class CreateRestaurantEndpoint(IMediator _mediator) : Endpoint<CreateRestaurantCommand, int>
{    
    public override void Configure()
    {
        Post("api/restaurants");  
    }

    public override async Task HandleAsync(CreateRestaurantCommand req, CancellationToken ct)
    {
        var id = await _mediator.Send(req);
        await SendCreatedAtAsync<GetRestaurantByIdEndpoint>(new { id }, 0, cancellation: ct);
    }
}

public class UpdateRestaurantEndpoint(IMediator _mediator) : Endpoint<UpdateRestaurantCommand>
{
   
    public override void Configure()
    {
        Patch("api/restaurants/{id}");
        Roles(UserRoles.Owner, UserRoles.Admin, UserRoles.User);
    }

    public override async Task HandleAsync(UpdateRestaurantCommand req, CancellationToken ct)
    {
        req.Id = Route<int>("id");
        await _mediator.Send(req);
        await SendNoContentAsync(ct);
    }
}

public class DeleteRestaurantEndpoint(IMediator _mediator) : EndpointWithoutRequest
{
   

    public override void Configure()
    {
        Delete("api/restaurants/{id}");
        Roles(UserRoles.Owner);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        await _mediator.Send(new DeleteRestaurantCommand(id));
        await SendNoContentAsync(ct);
    }
}
