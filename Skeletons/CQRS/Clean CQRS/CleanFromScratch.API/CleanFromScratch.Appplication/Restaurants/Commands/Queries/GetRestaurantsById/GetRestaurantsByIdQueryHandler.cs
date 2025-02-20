using AutoMapper;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Exceptions;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetRestaurantsById;

public class GetRestaurantsByIdQueryHandler(ILogger<GetRestaurantsByIdQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantsByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantsByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant by id {RestaurantId}",request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id) 
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        //RestaurantDto.FromEntity(restaurant);
        return restaurantDto;
    }
}
