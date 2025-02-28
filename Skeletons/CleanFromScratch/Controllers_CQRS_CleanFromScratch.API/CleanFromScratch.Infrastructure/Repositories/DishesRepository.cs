﻿using CleanFromScratch.Domain.Entities;
using CleanFromScratch.Domain.Repositories;
using CleanFromScratch.Infrastructure.Persistence;

namespace CleanFromScratch.Infrastructure.Repositories;

internal class DishesRepository(RestaurantDbContext dbContext) : IDishesRepository
{
    public async Task<int> Create(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(IEnumerable<Dish> entities)
    {
        dbContext.Dishes.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }
}