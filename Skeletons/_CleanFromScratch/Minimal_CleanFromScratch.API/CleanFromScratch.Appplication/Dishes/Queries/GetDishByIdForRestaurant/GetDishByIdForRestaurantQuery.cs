
﻿using MediatR;
using CleanFromScratch.Appplication.Dishes.Dtos;

namespace CleanFromScratch.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}