using CleanFromScratch.Application.Dishes.Commands.CreateDish;
using CleanFromScratch.Application.Dishes.Commands.DeleteDishes;
using CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;
using CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;
using CleanFromScratch.Appplication.Dishes.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Controllers;

[Route("api/restaurant/{restaurantId}/dishes")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;

        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, dishId }, null);
    }

    [HttpGet] 
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDishesForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
        return NoContent();
    }
}
