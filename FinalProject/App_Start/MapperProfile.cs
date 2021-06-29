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
            Mapper.CreateMap<Prescription, FormPrescriptionDTO>();
            Mapper.CreateMap<Prescription, PrescriptionDTO>();


            Mapper.CreateMap<Medicine, MedicineDTO>();
            Mapper.CreateMap<MedicineType, MedicineTypeDTO>();
            Mapper.CreateMap<Certifcate, CertificateDTO>();
            Mapper.CreateMap<ClinicType, ClinicTypeDTO>();
            Mapper.CreateMap<Clinic, ClinicDTO>();
            Mapper.CreateMap<Vacation, VacationDTO>();
            Mapper.CreateMap<Clinic, ClinicOnlyDTO>();
            Mapper.CreateMap<Appointment, AppointmentDTO>();
            Mapper.CreateMap<Clinic, ClinicInfoDTO>()
                .ForMember(c => c.Doctor, o => o.MapFrom(c => c.ForUser));

            Mapper.CreateMap<Clinic, MapObject>()
                .ForMember(c => c.Longitude, s => s.MapFrom(c => c.Location.Longtude))
                .ForMember(c => c.ObjectName, s => s.MapFrom(c => c.ClinicName))
                .ForMember(c => c.ObjectId, s => s.MapFrom(c => c.ClinicId))
                .ForMember(c => c.Latitude, s => s.MapFrom(c => c.Location.Latitude));

            Mapper.CreateMap<Pharmacy, PharmacyDTO>();
            Mapper.CreateMap<Pharmacy, PharmacyOnlyDTO>();
            Mapper.CreateMap<Pharmacy, CreatePharmacyDTO>();
            Mapper.CreateMap<Pharmacy, MapObject>()
                .ForMember(c => c.Longitude, s => s.MapFrom(c => c.Longtude))
                .ForMember(c => c.ObjectName, s => s.MapFrom(c => c.PharmacyName))
                .ForMember(c => c.ObjectId, s => s.MapFrom(c => c.PharmacyId))
                .ForMember(c => c.Latitude, s => s.MapFrom(c => c.Latitude)); 

            //To Domain Classes
            Mapper.CreateMap<UserDTO, User>()
                .ForMember(u => u.UserId, op => op.Ignore());

            Mapper.CreateMap<CreateClinicDTO, Clinic>();
            Mapper.CreateMap<CreateClinicViewModel, Clinic>();

            Mapper.CreateMap<ClinicTypeDTO, ClinicType>();
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

            Mapper.CreateMap<CertificateDTO, Certifcate>()
                .ForMember(c => c.CertifcateID, op => op.Ignore());

            Mapper.CreateMap<PharmacyOnlyDTO, Pharmacy>()
                .ForMember(u => u.PharmacyId, op => op.Ignore())
                .ForMember(c => c.ForUser, op => op.Ignore());

            Mapper.CreateMap<FormPrescriptionDTO, Prescription>();

            Mapper.CreateMap<MedicineTypeDTO, MedicineType>()
                .ForMember(m => m.MedicineTypeId, op => op.Ignore());

            Mapper.CreateMap<MedicineDTO, Medicine>()
                .ForMember(m => m.MedicineId, op => op.Ignore());

            Mapper.CreateMap<CreatePharmacyDTO, Pharmacy>();

            Mapper.CreateMap<SignUpUser, User>()
                .ForMember(s => s.UserId, op => op.Ignore());

            Mapper.CreateMap<ScheduleDTO, Schedule>()
                .ForMember(s => s.ClinicId, op => op.Ignore());
            Mapper.CreateMap<LocationDTO, Location>();
            Mapper.CreateMap<VacationDTO, Vacation>();
        }
    }
}