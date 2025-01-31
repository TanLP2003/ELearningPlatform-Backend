using AutoMapper;
using UserService.API.DTOs;

namespace UserService.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileInfoDTO, UserService.API.Models.Profile>().ReverseMap();              
        }
    }
}
