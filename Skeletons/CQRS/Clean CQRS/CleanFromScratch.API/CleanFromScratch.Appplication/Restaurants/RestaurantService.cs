using AutoMapper;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CleanFromScratch.Appplication.Restaurants
{
    public class RestaurantService(IRestaurantsRepository restaurantsRepository, 
        ILogger<RestaurantService> logger,
        IMapper mapper) : IRestaurantService
    {
     

        public async Task<IEnumerable<RestaurantDto>> GetAll()
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllAsync();

            var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            //restaurants.Select(RestaurantDto.FromEntity);

            return restaurantDtos!;
        }

        public async Task<RestaurantDto> GetById(int id)
        {
            logger.LogInformation("Getting restaurant by id");
            var restaurant = await restaurantsRepository.GetByIdAsync(id);
            var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
            //RestaurantDto.FromEntity(restaurant);
            return restaurantDto;
        }
    }
}
