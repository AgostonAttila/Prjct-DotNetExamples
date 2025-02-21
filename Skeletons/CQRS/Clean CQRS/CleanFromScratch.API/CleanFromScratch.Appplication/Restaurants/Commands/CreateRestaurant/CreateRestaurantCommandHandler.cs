using AutoMapper;
using CleanFromScratch.Application.Users;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanFromScratch.Appplication.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserName} {UserId} Creating a new restaurant {Restaurant}",
            currentUser.Email,
            currentUser.Id,
            request);

        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;

        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }
}
