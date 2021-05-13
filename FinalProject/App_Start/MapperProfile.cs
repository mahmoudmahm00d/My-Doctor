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
            Mapper.CreateMap<User, Doctor>();

            Mapper.CreateMap<Location, LocationDTO>();
            Mapper.CreateMap<Schedule, ScheduleDTO>();

            Mapper.CreateMap<ScheduleDTO, Schedule>();
            Mapper.CreateMap<LocationDTO, Location>();
        }
    }
}