using AutoMapper;
using Controllers.Application.DTOs;
using Controllers.Domain.Entities;

namespace Controllers.Application.Common
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Domain to DTO
            CreateMap<User, UserDto>();

            // DTO to Domain
            CreateMap<UserDto, User>();
        }
    }
}
