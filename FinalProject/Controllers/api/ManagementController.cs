using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace FinalProject.Controllers.api
{
    //Add Authrization
    public class ManagementController : ApiController
    {
        private MyAppContext db = new MyAppContext();
        // GET api/<controller>
        #region Sign Up Request

        [HttpGet]
        [Route("api/Management/SignUpRequests")]
        public IHttpActionResult SignUpRequests()
        {
            var signUpRequests = db.Clinics.Include(c => c.ForUser).Include(c => c.ClinicType).Where(c => c.IsActiveClinic == false).Select(c => c).ToList();
            if (signUpRequests.Count == 0)
            {
                return Ok(signUpRequests);
            }


            var result = signUpRequests.Select(r => new
            {
                clinicId = r.ClinicId,
                clinicName = r.ClinicName,
                clinicType = r.ClinicType.ClinicTypeName,
                doctorName = $"{r.ForUser.FirstName} {r.ForUser.LastName}",
                certificate = r.Certificate
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Management/Clinics/Accept/{id}")]
        public IHttpActionResult AcceptClinic(int id)
        {
            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id && c.IsActiveClinic == false);
            if (clinic == null)
                return NotFound();

            clinic.IsActiveClinic = true;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/Management/Clinics/Refuse/{id}")]
        public IHttpActionResult RefuseClinic(int id)
        {
            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id && c.IsActiveClinic == false);
            if (clinic == null)
                return NotFound();

            //ToDo
            //Send email contains why
            var location = db.Locations.FirstOrDefault(l => l.ClinicId == clinic.ClinicId);
            var schedule = db.Schedules.FirstOrDefault(s => s.ClinicId == clinic.ClinicId);
            db.Locations.Remove(location);
            db.Schedules.Remove(schedule);
            db.Clinics.Remove(clinic);
            db.SaveChanges();
            return Ok();
        }

        #endregion

        #region Clinics

        [HttpGet]
        [Route("api/Management/Clinics")]
        public IHttpActionResult GetClinic()
        {
            var clinics = db.Clinics.Include(c => c.ForUser).Include(c=>c.ClinicType).ToList();
            if (clinics.Count == 0)
                return Ok(clinics);//Retune empty list

            var result = clinics.Select(r => new
            {
                clinicId = r.ClinicId,
                clinicName = r.ClinicName,
                clinicType = r.ClinicType.ClinicTypeName,
                doctorName = $"{r.ForUser.FirstName} {r.ForUser.LastName}",
                certificate = r.Certificate,
                isActiveClinic = r.IsActiveClinic,
                doctorId = r.UserId
            });

            return Ok(result);
        }

        #endregion

        #region Cities

        [HttpGet]
        [Route("api/Management/Cities")]
        public IHttpActionResult GetCities()
        {
            return Ok(db.Cities.Select(Mapper.Map<City,CityDTO>).ToList());
        }


        // GET api/<controller>/5
        [HttpGet]
        [Route("api/Management/Cities/{id}")]
        public IHttpActionResult GetCity(int id)
        {
            var city = db.Cities.FirstOrDefault(c => c.CityId == id);
            if (city == null)
                return NotFound();

            return Ok( Mapper.Map<City,CityDTO>(city));
        }

        #endregion

        #region Medicines

        [HttpGet]
        [Route("api/Management/Medicines")]
        public IHttpActionResult GetMedicines()
        {
            var medicines = db.Medicines.Include(m => m.MedicineType).ToList();
            if (medicines.Count() == 0)
                return Ok(medicines);//Empty list

            var result = medicines.Select(m => new
            {
                medicineId = m.MedicineId,
                nameAR = m.NameAR,
                nameEN = m.NameEN,
                medicineType = m.MedicineType.MedicineTypeName,
                isActiveMedicine = m.IsActiveMedicine
            });

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/Management/Medicines/{id}")]//PhramacyId
        public IHttpActionResult DeleteMedicine(int id)
        {
            var medicineInDb = db.Medicines.Find(id);
            if (medicineInDb == null)
                return NotFound();

            var medicine = db.PharmacyMedicines.Where(m => m.MedicineId == id).Select(m => m).ToList();
            if (medicine.Count() < 1)
            {
                var med = db.Medicines.FirstOrDefault(m => m.MedicineId == id);
                db.Medicines.Remove(med);
                db.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        #endregion

        #region Users

        [HttpGet]
        [Route("api/Management/Users")]
        public IHttpActionResult GetUsers()
        {
            var users = db.Users.Include(u => u.UserType).ToList();
            if (users.Count() == 0)
                return Ok(users);

            var result = users.Select(u => new
            {
                userId = u.UserId,
                firstName = u.FirstName,
                fatherName = u.FatherName,
                lastName = u.LastName,
                userType = u.UserType.UserTypeName,
                locked = u.Locked
            });

            return Ok(result);
        }

        #endregion
    }
}