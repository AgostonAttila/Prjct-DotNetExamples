using CleanFromScratch.Application.Dishes.Commands.CreateDish;
using CleanFromScratch.Application.Dishes.Commands.DeleteDishes;
using CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;
using CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;
using CleanFromScratch.Appplication.Dishes.Dtos;
using FastEndpoints;
using MediatR;

namespace CleanFromScratch.API.Endpoints;

public class CreateDishEndpoint(IMediator _mediator) : Endpoint<CreateDishCommand, int>
{
   
    public override void Configure()
    {
        Post("api/restaurant/{restaurantId}/dishes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateDishCommand req, CancellationToken ct)
    {
        req.RestaurantId = Route<int>("restaurantId");
        var dishId = await _mediator.Send(req);
        await SendCreatedAtAsync<GetDishByIdForRestaurantEndpoint>(new { restaurantId = req.RestaurantId, dishId }, 0, cancellation: ct);
    }
}

public class GetAllDishesForRestaurantEndpoint(IMediator _mediator) : EndpointWithoutRequest<IEnumerable<DishDto>>
{
 
    public override void Configure()
    {
        Get("api/restaurant/{restaurantId}/dishes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var restaurantId = Route<int>("restaurantId");
        var dishes = await _mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        await SendOkAsync(dishes, ct);
    }
}

public class GetDishByIdForRestaurantEndpoint(IMediator _mediator) : EndpointWithoutRequest<DishDto>
{
  
    public override void Configure()
    {
        Get("api/restaurant/{restaurantId}/dishes/{dishId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var restaurantId = Route<int>("restaurantId");
        var dishId = Route<int>("dishId");
        var dish = await _mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        await SendOkAsync(dish, ct);
    }
}

public class DeleteDishesForRestaurantEndpoint(IMediator _mediator) : EndpointWithoutRequest
{  
    public override void Configure()
    {
        Delete("api/restaurant/{restaurantId}/dishes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var restaurantId = Route<int>("restaurantId");
        await _mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
        await SendNoContentAsync(ct);
    }
}
