﻿using MediatR;
using Microsoft.Extensions.Logging;
using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Exceptions;
using CleanFromScratch.Domain.Repositories;

namespace CleanFromScratch.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository
    ) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Removing all dishes from restaurant: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        //if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        //    throw new ForbidException();

        await dishesRepository.Delete(restaurant.Dishes);
    }
}