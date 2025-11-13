using AutoMapper;
using BusinessObjects;
using BusinessObjects.Dto;

namespace PRN232_Project_API.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Accusation, AccusationDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
