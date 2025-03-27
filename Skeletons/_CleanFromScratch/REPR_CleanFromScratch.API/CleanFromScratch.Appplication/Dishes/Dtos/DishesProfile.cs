using AutoMapper;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Appplication.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
    }
}
