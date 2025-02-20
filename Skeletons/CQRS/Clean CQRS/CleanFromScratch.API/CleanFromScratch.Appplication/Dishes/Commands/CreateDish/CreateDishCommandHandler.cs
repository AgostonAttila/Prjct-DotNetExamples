﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanFromScratch.Domain.Constants;
using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Exceptions;
using CleanFromScratch.Domain.Repositories;

namespace CleanFromScratch.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper
    ) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish: {@DishRequest}", request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        //if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        //    throw new ForbidException();

        var dish = mapper.Map<Dish>(request);

        return await dishesRepository.Create(dish);

    }
}