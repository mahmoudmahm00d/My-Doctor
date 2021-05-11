using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.App_Start
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<UserDTO, User>();
            Mapper.CreateMap<SignUpUser, User>();
        }
    }
}