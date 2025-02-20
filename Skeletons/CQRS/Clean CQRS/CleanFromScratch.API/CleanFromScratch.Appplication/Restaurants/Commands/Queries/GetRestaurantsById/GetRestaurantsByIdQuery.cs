using CleanFromScratch.Appplication.Restaurants.Dtos;
using MediatR;

namespace CleanFromScratch.Appplication.Restaurants.Commands.Queries.GetRestaurantsById;

public class GetRestaurantsByIdQuery(int id) : IRequest<RestaurantDto>    
{
    
    public int Id { get; } = id;
}
