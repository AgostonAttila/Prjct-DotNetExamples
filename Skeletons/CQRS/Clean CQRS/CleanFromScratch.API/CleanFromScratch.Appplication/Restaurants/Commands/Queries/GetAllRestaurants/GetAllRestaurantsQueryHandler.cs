using AutoMapper;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        //restaurants.Select(RestaurantDto.FromEntity);

        return restaurantDtos!;
    }
}
