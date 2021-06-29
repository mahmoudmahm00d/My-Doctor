using FinalProject.Models;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Authentication;

namespace FinalProject.Controllers.api
{
    public class MedicinesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        //Get Medicine By Name
        [HttpGet]
        [Route("api/medicine/name/{query}")]
        public IHttpActionResult Get(string query = null)
        {
            var medicineQuery = db.Medicines.Include(m=>m.MedicineType);
            if (!string.IsNullOrWhiteSpace(query))
            {
                medicineQuery = medicineQuery.Where(m => m.NameAR.Contains(query) && m.IsActiveMedicine);
            }

            var medicines = medicineQuery.ToList().Select(Mapper.Map<Medicine,MedicineDTO>);
            return Ok(medicines);
        }

        [HttpGet]
        [Route("api/medicine/users/{id}")]
        [UsersAuthentication]
        public IHttpActionResult GetMedcinesForUser(int? id)//User Id
        {
            if (!id.HasValue)
                return BadRequest();

            var query = from prescription in db.Prescriptions
                        join appointment in db.Appointments on prescription.AppointmentId equals appointment.AppointmentId
                        join clinic in db.Clinics on appointment.ClinicId equals clinic.ClinicId
                        join doctor in db.Users on clinic.UserId equals doctor.UserId
                        join clinicType in db.ClinicTypes on clinic.ClinicTypeId equals clinicType.ClinicTypeId
                        join medicine in db.Medicines on prescription.MedicineId equals medicine.MedicineId
                        join medicineType in db.MedicineTypes on medicine.MedicineTypeId equals medicineType.MedicineTypeId
                        where appointment.UserId == id.Value
                        select new
                        {
                            ClinicName = clinic.ClinicName,
                            ClinicType = clinicType.ClinicTypeName,
                            DoctorName = doctor.FirstName+" "+doctor.LastName,
                            Date = appointment.Date,
                            CompositId = prescription.AppointmentId + "," + prescription.MedicineId,
                            Dosage = prescription.Dosage,
                            Every = prescription.Every,
                            For = prescription.For,
                            MedicineNameAr = medicine.NameAR,
                            MedicineNameEn = medicine.NameEN,
                            MedicineType = medicineType.MedicineTypeName,
                            Timespan = prescription.TimeSpan
                        };

            return Ok(query);
        }
    }
}
