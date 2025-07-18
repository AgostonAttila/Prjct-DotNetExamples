﻿using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanFromScratch.Domain.Exceptions;
using CleanFromScratch.Domain.Interfaces;
using CleanFromScratch.Domain.Constants;

namespace CleanFromScratch.Appplication.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
IRestaurantsRepository restaurantsRepository,
IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting a restaurant with id: {REstaurantId}",request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await restaurantsRepository.Delete(restaurant);       
    }
}
