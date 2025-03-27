using AutoMapper;
using CleanFromScratch.Application.Common;
using CleanFromScratch.Appplication.Restaurants.Dtos;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFromScratch.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var (restaurants ,totalCount)= await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase,
            request.PageSize,
            request.PageNumber,
            request.SortBy,
            request.SortDirection
            );

        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
       
        var result = new PagedResult<RestaurantDto>(restaurantDtos, totalCount, request.PageSize, request.PageNumber);
        

        return result!;
    }
}
