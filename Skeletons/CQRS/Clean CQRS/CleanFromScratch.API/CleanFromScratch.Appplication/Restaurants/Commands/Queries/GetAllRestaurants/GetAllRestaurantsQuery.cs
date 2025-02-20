using CleanFromScratch.Appplication.Restaurants.Dtos;
using MediatR;

namespace CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery: IRequest<IEnumerable<RestaurantDto>>
{


}
