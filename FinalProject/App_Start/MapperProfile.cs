using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.App_Start
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<User, Doctor>();

            Mapper.CreateMap<Location, LocationDTO>();
            Mapper.CreateMap<Schedule, ScheduleDTO>();

            Mapper.CreateMap<Clinic, ClinicDTO>();
            Mapper.CreateMap<Clinic, ClinicOnlyDTO>();

            Mapper.CreateMap<Pharmacy, PharmacyDTO>();
            Mapper.CreateMap<Pharmacy, PharmacyOnlyDTO>();

            //To Domain Classes
            Mapper.CreateMap<UserDTO, User>()
                .ForMember(u => u.UserId, op => op.Ignore());

            Mapper.CreateMap<ClinicDTO, Clinic>()
                .ForMember(c => c.ForUser, op => op.Ignore())
                .ForMember(c => c.ClinicId, op => op.Ignore());

            Mapper.CreateMap<ClinicOnlyDTO, Clinic>()
                .ForMember(u => u.ClinicId, op => op.Ignore())
                .ForMember(c => c.ForUser, op => op.Ignore());

            Mapper.CreateMap<PharmacyDTO, Pharmacy>()
                .ForMember(c => c.ForUser, op => op.Ignore())
                .ForMember(c => c.PharmacyId, op => op.Ignore());

            Mapper.CreateMap<PharmacyOnlyDTO, Pharmacy>()
                .ForMember(u => u.PharmacyId, op => op.Ignore())
                .ForMember(c => c.ForUser, op => op.Ignore());

            Mapper.CreateMap<SignUpUser, User>()
                .ForMember(s => s.UserId, op => op.Ignore());

            Mapper.CreateMap<ScheduleDTO, Schedule>()
                .ForMember(s => s.ClinicId, op => op.Ignore());
            Mapper.CreateMap<LocationDTO, Location>()
                .ForMember(s => s.ClinicId, op => op.Ignore());
        }
    }
}