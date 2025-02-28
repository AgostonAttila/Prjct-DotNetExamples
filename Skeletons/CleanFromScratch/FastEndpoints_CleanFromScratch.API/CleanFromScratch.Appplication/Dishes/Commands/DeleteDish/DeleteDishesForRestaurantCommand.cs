﻿using MediatR;

namespace CleanFromScratch.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
