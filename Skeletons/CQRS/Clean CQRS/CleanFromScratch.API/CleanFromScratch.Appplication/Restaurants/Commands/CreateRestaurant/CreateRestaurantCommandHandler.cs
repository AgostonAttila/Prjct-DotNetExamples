using AutoMapper;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new restaurant {Restaurant}",request);

        var restaurant = mapper.Map<Restaurant>(request);

        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }
}
