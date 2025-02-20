using AutoMapper;
using CleanFromScratch.Domain.Entities;

namespace CleanFromScratch.Appplication.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<CreateRestaurantDto,Restaurant>()
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode
                }))
            .ForMember(dest => dest.Dishes, opt => opt.Ignore());

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(dest => dest.Street,
                opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(dest => dest.PostalCode,
                opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(dest => dest.Dishes, 
                opt => opt.MapFrom(src => src.Dishes));



    }
}
