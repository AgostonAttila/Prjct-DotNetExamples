using AutoMapper;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Appplication.Dishes.Dtos;

public class DishesPRofile : Profile
{
    public DishesPRofile()
    {
        CreateMap<Dish, DishDto>();
    }
}
