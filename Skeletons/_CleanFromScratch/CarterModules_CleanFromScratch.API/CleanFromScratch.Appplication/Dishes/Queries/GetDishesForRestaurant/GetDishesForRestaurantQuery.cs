﻿using MediatR;
using CleanFromScratch.Appplication.Dishes.Dtos;

namespace CleanFromScratch.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = restaurantId;
}