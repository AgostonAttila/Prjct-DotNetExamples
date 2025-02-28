using CleanFromScratch.Appplication.Restaurants;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CleanFromScratch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await restaurantService.GetAll();
            return Ok(restaurants);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var restaurant = await restaurantService.GetById(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto restaurantDto)
        {          

            int id = await restaurantService.Create(restaurantDto);        
            return CreatedAtAction(nameof(GetById), new { id},null);
        }
    }
}
