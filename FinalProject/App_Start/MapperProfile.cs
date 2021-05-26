using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.ViewModels;

namespace FinalProject.App_Start
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<UserType, UserTypeDTO>();
            Mapper.CreateMap<User, UsersManageViewModel>();
            Mapper.CreateMap<User, Doctor>();
            Mapper.CreateMap<object, User>();

            Mapper.CreateMap<City, CityDTO>();
            Mapper.CreateMap<Location, LocationDTO>();
            Mapper.CreateMap<Schedule, ScheduleDTO>();

            Mapper.CreateMap<Clinic, ClinicDTO>();
            Mapper.CreateMap<Clinic, ClinicOnlyDTO>();
            Mapper.CreateMap<Appointment, AppointmentDTO>();

            //ToDo
            Mapper.CreateMap<Clinic, MapObject>()
                .ForMember(c => c.Langtude , s => s.MapFrom(c => c.Location.Langtude))
                .ForMember(c => c.Latitude, s => s.MapFrom(c => c.Location.Latitude));

            Mapper.CreateMap<Pharmacy, PharmacyDTO>();
            Mapper.CreateMap<Pharmacy, PharmacyOnlyDTO>();
            Mapper.CreateMap<Pharmacy, MapObject>();

            //To Domain Classes
            Mapper.CreateMap<UserDTO, User>()
                .ForMember(u => u.UserId, op => op.Ignore());

            Mapper.CreateMap<UserTypeDTO, UserType>()
                .ForMember(u => u.UserTypeId, op => op.Ignore());

            Mapper.CreateMap<ClinicDTO, Clinic>()
                .ForMember(c => c.ForUser, op => op.Ignore())
                .ForMember(c => c.ClinicId, op => op.Ignore());

            Mapper.CreateMap<ClinicOnlyDTO, Clinic>()
                .ForMember(u => u.ClinicId, op => op.Ignore())
                .ForMember(c => c.ForUser, op => op.Ignore());

            Mapper.CreateMap<PharmacyDTO, Pharmacy>()
                .ForMember(c => c.ForUser, op => op.Ignore())
                .ForMember(c => c.PharmacyId, op => op.Ignore());

            Mapper.CreateMap<CityDTO, City>()
                .ForMember(u => u.CityId, op => op.Ignore());

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