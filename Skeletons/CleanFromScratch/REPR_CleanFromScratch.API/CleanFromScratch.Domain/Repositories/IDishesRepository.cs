﻿using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> Create(Dish entity);
    Task Delete(IEnumerable<Dish> entities);
}