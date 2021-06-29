using AutoMapper;
using FinalProject.Authentication;
using FinalProject.DTOs;
using FinalProject.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    //Add Authrization
    [ManagerAuthentication]
    public class ManagementController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        #region Sign Up Request
        [HttpGet]
        [Route("api/Management/PharmaciesSignUpRequests")]
        public IHttpActionResult PharmaciesSignUpRequests()
        {
            var signUpRequestsClinics = db.Clinics.Include(c => c.ForUser).Include(c => c.ClinicType).Where(c => c.IsActiveClinic == false).Select(c => c);
            var signUpRequestsPharamacies = db.Pharmacies.Include(c => c.ForUser).Where(c => c.IsActivePharmacy == false).Select(c => c);

            if (signUpRequestsClinics.Count() == 0 && signUpRequestsPharamacies.Count() == 0)
            {
                return Ok(new List<SignUpRequestDTO>());//Empty List
            }

            var pharmacies = signUpRequestsPharamacies.Select(pharmacy => new SignUpRequestDTO
            {
                Id = pharmacy.PharmacyId,
                Name = pharmacy.PharmacyName,
                Type = "Phamracy",
                DoctorName = pharmacy.ForUser.FirstName + " " + pharmacy.ForUser.LastName,
                Certificate = pharmacy.Certificate
            });

            return Ok(pharmacies.ToList());
        }

        [HttpGet]
        [Route("api/Management/ClinicsSignUpRequests")]
        public IHttpActionResult ClinicsSignUpRequests()
        {
            var signUpRequestsClinics = db.Clinics.Include(c => c.ForUser).Include(c => c.ClinicType).Where(c => c.IsActiveClinic == false).Select(c => c);
            var signUpRequestsPharamacies = db.Pharmacies.Include(c => c.ForUser).Where(c => c.IsActivePharmacy == false).Select(c => c);

            if (signUpRequestsClinics.Count() == 0 && signUpRequestsPharamacies.Count() == 0)
            {
                return Ok(new List<SignUpRequestDTO>());//Empty List
            }

            var clinics = signUpRequestsClinics.Select(clinic => new SignUpRequestDTO
            {
                Id = clinic.ClinicId,
                Name = clinic.ClinicName,
                Type = clinic.ClinicType.ClinicTypeName,
                DoctorName = clinic.ForUser.FirstName + " " + clinic.ForUser.LastName,
                Certificate = clinic.Certificate
            });

            return Ok(clinics.ToList());
        }

        [HttpPost]
        [Route("api/Management/Clinics/AcceptClinic/{id}")]
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
        [Route("api/Management/Clinics/RefuseClinic/{id}")]
        public IHttpActionResult RefuseClinic(int id)
        {
            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id && c.IsActiveClinic == false);
            if (clinic == null)
                return NotFound();

            //ToDo
            //Send email contains why
            var location = db.Locations.FirstOrDefault(l => l.ClinicId == clinic.ClinicId);
            var schedule = db.Schedules.FirstOrDefault(s => s.ClinicId == clinic.ClinicId);
            if (location != null)
                db.Locations.Remove(location);
            if (schedule != null)
                db.Schedules.Remove(schedule);
            db.Clinics.Remove(clinic);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/Management/Clinics/AcceptPharmacy/{id}")]
        public IHttpActionResult AcceptPharmacy(int id)
        {
            var pharmacy = db.Pharmacies.FirstOrDefault(c => c.PharmacyId == id && c.IsActivePharmacy == false);
            if (pharmacy == null)
                return NotFound();

            pharmacy.IsActivePharmacy = true;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/Management/Clinics/RefusePharmacy/{id}")]
        public IHttpActionResult RefusePharmacy(int id)
        {
            var pharamcy = db.Pharmacies.FirstOrDefault(p => p.PharmacyId == id && p.IsActivePharmacy == false);
            if (pharamcy == null)
                return NotFound();

            //ToDo
            //Send email contains why
            db.Pharmacies.Remove(pharamcy);
            db.SaveChanges();
            return Ok();
        }

        #endregion

        #region Clinics

        [HttpGet]
        [Route("api/Management/Clinics")]
        public IHttpActionResult GetClinic()
        {
            var clinics = db.Clinics.Include(c => c.ForUser).Include(c => c.ClinicType).ToList();
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

        #region Pharmacies

        [HttpGet]
        [Route("api/Management/Pharmacies")]
        public IHttpActionResult GetPharmacies()
        {
            var pharmacies = db.Pharmacies.Include(c => c.ForUser).ToList();
            if (pharmacies.Count == 0)
                return Ok(pharmacies);//Retune empty list

            var result = pharmacies.Select(phramacy => new
            {
                phramacyId = phramacy.PharmacyId,
                phramacyName = phramacy.PharmacyName,
                doctorName = $"{phramacy.ForUser.FirstName} {phramacy.ForUser.LastName}",
                certificate = phramacy.Certificate,
                isActivePharmacy = phramacy.IsActivePharmacy,
                doctorId = phramacy.UserId
            });

            return Ok(result);
        }

        #endregion

        #region ClinicTypes
        [HttpDelete]
        [Route("api/Management/ClinicTypes/{id}")]
        public IHttpActionResult GetClinicTypes(int id)
        {
            var clinicType = db.ClinicTypes.FirstOrDefault(c => c.ClinicTypeId == id);
            if (clinicType == null)
                return NotFound();

            var clinics = db.Clinics.Where(c => c.ClinicTypeId == id);
            if (clinics.Count() != 0)
                return BadRequest();

            db.ClinicTypes.Remove(clinicType);
            db.SaveChanges();
            return Ok(Mapper.Map<ClinicType, ClinicTypeDTO>(clinicType));
        }

        [HttpGet]
        [Route("api/Management/ClinicTypes")]
        public IHttpActionResult GetClinicTypes()
        {
            return Ok(db.ClinicTypes.Select(Mapper.Map<ClinicType, ClinicTypeDTO>).ToList());
        }
        #endregion

        #region MedicineTypes
        [HttpDelete]
        [Route("api/Management/MedicineTypes/{id}")]
        public IHttpActionResult GetMedicineTypes(int id)
        {
            var MedicineType = db.MedicineTypes.FirstOrDefault(c => c.MedicineTypeId == id);
            if (MedicineType == null)
                return NotFound();

            var Medicines = db.Medicines.Where(c => c.MedicineTypeId == id);
            if (Medicines.Count() != 0)
                return BadRequest();

            db.MedicineTypes.Remove(MedicineType);
            db.SaveChanges();
            return Ok(Mapper.Map<MedicineType, MedicineTypeDTO>(MedicineType));
        }

        [HttpGet]
        [Route("api/Management/MedicineTypes")]
        public IHttpActionResult GetMedicineTypes()
        {
            return Ok(db.MedicineTypes.Select(Mapper.Map<MedicineType, MedicineTypeDTO>).ToList());
        }
        #endregion

        #region Cities

        [HttpGet]
        [Route("api/Management/Cities")]
        public IHttpActionResult GetCities()
        {
            return Ok(db.Cities.Select(Mapper.Map<City, CityDTO>).ToList());
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/Management/Cities/{id}")]
        public IHttpActionResult GetCity(int id)
        {
            var city = db.Cities.FirstOrDefault(c => c.CityId == id);
            if (city == null)
                return NotFound();

            return Ok(Mapper.Map<City, CityDTO>(city));
        }

        [HttpDelete]
        [Route("api/Management/Cities/{id}")]
        public IHttpActionResult DeleteCity(int id)
        {
            var city = db.Cities.FirstOrDefault(c => c.CityId == id);
            if (city == null)
                return NotFound();

            var locations = db.Locations.Where(l => l.CityId == id);
            if (locations.Count() != 0)
                return BadRequest();

            db.Cities.Remove(city);
            db.SaveChanges();
            return Ok(Mapper.Map<City, CityDTO>(city));
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