using CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;
using CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetAllRestaurants;
using CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetRestaurantsById;
using CleanFromScratch.Appplication.Restaurants.Commands.UpdateRestaurantCommand;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantsByIdQuery(id));           
            return Ok(restaurant);
        }


        [HttpPost]
        public async Task<ActionResult<int>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {

            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRestaurant([FromBody] int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);

            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));

            return NoContent();
        }
    }
}
