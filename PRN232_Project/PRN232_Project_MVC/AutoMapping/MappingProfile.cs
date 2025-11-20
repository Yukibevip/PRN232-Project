using AutoMapper;
using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.Models.Dto;

namespace PRN232_Project_MVC.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Accusation, AccusationDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<FriendList, FriendListDto>().ReverseMap();
            CreateMap<FriendInvitation, FriendInvitationDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<BlockList, BlockListDto>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();
        }
    }
}
