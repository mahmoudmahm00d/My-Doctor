using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class ClinicsController : Controller
    {
        private MyAppContext db = new MyAppContext();

        // GET: Clinics
        public ActionResult Index()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int userId = GetUserId();
            var clinic = db.Clinics.FirstOrDefault(c => c.UserId == userId);
            if (clinic == null)
                return RedirectToAction("Create");

            Session["ClinicId"] = clinic.ClinicId;

            return View(clinic.IsActiveClinic);
        }

        public ActionResult Create()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int userId = GetUserId();
            var Clinic = db.Clinics.Select(c => c.UserId == userId);

            if (Clinic == null)
                return RedirectToAction("Index");

            var clinicTypes = db.ClinicTypes.Where(c => c.IsActiveClinicType == true).Select(Mapper.Map<ClinicType, ClinicTypeDTO>);
            CreateClinicViewModel model = new CreateClinicViewModel { Clinics = clinicTypes };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateClinicDTO clinic, HttpPostedFileBase certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid || certificate == null || (certificate.ContentLength == 0 && certificate.ContentLength > 204800))//200KB
            {
                return RedirectToAction("Create");
            }

            string extention = Path.GetExtension(certificate.FileName);
            if (extention != ".png")
            {
                return RedirectToAction("Create");
            }

            var clinicInDB = Mapper.Map<CreateClinicDTO, Clinic>(clinic);
            string path = Path.Combine(Server.MapPath("~/Certificates"), certificate.FileName);
            clinicInDB.UserId = GetUserId();
            certificate.SaveAs(path);
            clinicInDB.Certificate = "clinic"+clinicInDB.UserId;
            clinicInDB.VisitDuration = 15;
            clinicInDB.IsActiveClinic = false;

            db.Clinics.Add(clinicInDB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Clinics/AddDay
        public ActionResult AddDay()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            return View();
        }

        [HttpPost]
        public ActionResult AddDay(ScheduleDTO schedules)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Schedule");
                return View();
            }
            if (schedules.FromTime > schedules.ToTime)
            {
                ModelState.AddModelError("", "invalid schedule");
                return View();
            }

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var scheduleInDb = db.Schedules.Where(s => s.ClinicId == clinicId)
                .FirstOrDefault(s => s.Day == schedules.Day);

            IEnumerable<Schedule> schedulesInDb = db.Schedules.Where(s => s.ClinicId == clinicId
                && s.Day == schedules.Day
                && s.ScheduleId != schedules.ScheduleId)
                .Select(s => s);

            if (scheduleInDb != null && schedulesInDb.Count() != 0)
            {
                foreach (var day in schedulesInDb)
                {
                    if (Time.CheckTimeBetween(schedules, day))
                    {
                        ModelState.AddModelError("", "invalid schedule");
                        return View();
                    }
                }
            }

            //Convert Time To String
            Time fromTime = new Time(schedules.FromTime);
            Time toTime = new Time(schedules.ToTime);

            var schedule = Mapper.Map<ScheduleDTO, Schedule>(schedules);
            schedule.ClinicId = clinicId.Value;
            schedule.FromTime = fromTime.ToString();
            schedule.ToTime = toTime.ToString();

            db.Schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        //GET: Clinics/Appointments
        public ActionResult Appointments()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            return View();
        }

        //GET: Clinics/Schedule
        public ActionResult Schedule()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == clinicId);

            return View(clinic.VisitDuration);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VisitDuration(byte? visitDuration)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!visitDuration.HasValue)
            {
                ModelState.AddModelError("", "Duration Required");
            }

            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == clinicId);
            clinic.VisitDuration = visitDuration.Value;
            db.SaveChanges();

            return View("Schedule", clinic.VisitDuration);
        }

        //GET: Clinics/EditSchedule
        public ActionResult EditSchedule(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("index");

            var scheduleInDb = db.Schedules.FirstOrDefault(s => s.ScheduleId == id);
            if (scheduleInDb == null)
                return HttpNotFound();

            var schedule = Mapper.Map<Schedule, ScheduleDTO>(scheduleInDb);
            return View(schedule);
        }

        [HttpPost]
        public ActionResult EditSchedule(int? id, ScheduleDTO schedules)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue && id == schedules.ScheduleId)
                return RedirectToAction("Schedule");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Schedule");
                return View();
            }
            if (schedules.FromTime > schedules.ToTime)
            {
                ModelState.AddModelError("", "invalid schedule");
                return View();
            }

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var scheduleInDb = db.Schedules.FirstOrDefault(s => s.ScheduleId == id);
            //Get all schedules that in the day except this day
            IEnumerable<Schedule> schedulesInDb = db.Schedules.Where(s => s.ClinicId == clinicId
                && s.Day == schedules.Day
                && s.ScheduleId != schedules.ScheduleId)
                .Select(s => s);

            if (scheduleInDb != null && schedulesInDb.Count() != 0)
            {
                foreach (var day in schedulesInDb)
                {
                    if (Time.CheckTimeBetween(schedules, day))
                    {
                        ModelState.AddModelError("", "invalid schedule");
                        return View();
                    }
                }
            }

            Time fromTime = new Time(schedules.FromTime);
            Time toTime = new Time(schedules.ToTime);

            scheduleInDb.Day = schedules.Day;
            scheduleInDb.FromTime = fromTime.ToString();
            scheduleInDb.ToTime = toTime.ToString();

            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        //GET: Clinics/UpcommingAppointments
        public ActionResult UpcommingAppointments()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            return View();
        }

        //GET: Clinics/AppointmentDetails
        public ActionResult PatientAppointmentsDetails(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("Appointments");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointment = db.Appointments.Include(a => a.User).FirstOrDefault(a => a.AppointmentId == id
                  && a.ClinicId == clinicId);

            if (appointment == null)
                return HttpNotFound();

            string patientName = $"{appointment.User.FirstName} {appointment.User.LastName}";

            return View(new { patientName });
        }

        //GET: Clinics/AppointmentDetails
        public ActionResult AppointmentDetails(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("Appointments");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointment = db.Appointments.Include(a => a.User)
                .Where(a => a.AppointmentId == id
                && a.ClinicId == clinicId)
                .Select(Mapper.Map<Appointment, AppointmentDTO>)
                .FirstOrDefault();

            if (appointment == null)
                return HttpNotFound();


            return View(appointment);
        }

        //GET: Clinics/Location
        public ActionResult Location()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            var cities = db.Cities.Where(c => c.IsActiveCity == true).Select(Mapper.Map<City, CityDTO>);
            LocationViewModel model = new LocationViewModel { Cities = cities };

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var location = db.Locations.FirstOrDefault(l => l.ClinicId == clinicId);
            if (location != null)
                model.Location = Mapper.Map<Location, LocationDTO>(location);
            else
                model.Location = new LocationDTO { ClinicId = clinicId.Value };

            return View(model);
        }

        [HttpPost]
        public ActionResult Location(LocationDTO location)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Location");
                return RedirectToAction("Location");
            }

            var locationInDb = db.Locations.FirstOrDefault(l => l.ClinicId == clinicId);
            location = AppServices.TrimStringProperties(location);
            //locationInDb = Mapper.Map<LocationDTO, Location>(location);

            locationInDb.Latitude = location.Latitude;
            locationInDb.Longtude = location.Longtude;
            locationInDb.Street = location.Street;
            locationInDb.Area = location.Area;
            locationInDb.ClinicId = clinicId.Value;
            if (locationInDb == null)
            {
                db.Locations.Add(locationInDb);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Clinics/EditPrescription
        public ActionResult EditPrescription(string compositeId)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (string.IsNullOrWhiteSpace(compositeId))
                return RedirectToAction("AppointmentDetails");

            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            string[] compositeKey = compositeId.Split(',');
            var prescrition = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == int.Parse(compositeKey[0])
                && p.MedicineId == int.Parse(compositeKey[1]));

            return View(prescrition);
        }

        //GET: Clinics/Management
        public ActionResult Management()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            int doctorId = GetUserId();
            var clinic = db.Clinics
                .Include("ClinicType")
                .Where(c => c.ClinicId == clinicId)
                .Select(Mapper.Map<Clinic, ClinicOnlyDTO>)
                .FirstOrDefault();

            var doctor = db.Users
                .Where(u => u.UserId == doctorId)
                .Select(Mapper.Map<User, Doctor>)
                .FirstOrDefault();

            var certificates = db.Certifcates.Where(c => c.UserId == doctorId).Select(c => c);

            doctor.Certificates = certificates;//All Certificates

            ClinicManagementViewModel model = new ClinicManagementViewModel { Doctor = doctor, Clinic = clinic };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Management(ClinicManagementViewModel model)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var clinic = db.Clinics
                .Include("ClinicType")
                .Where(c => c.ClinicId == clinicId)
                .FirstOrDefault();

            int doctorId = GetUserId();
            var doctor = db.Users
                .Where(u => u.UserId == doctorId)
                .FirstOrDefault();

            model.Doctor = AppServices.TrimStringProperties(model.Doctor);
            model.Clinic = AppServices.TrimStringProperties(model.Clinic);
            if (ModelState.IsValid)
            {
                doctor.FirstName = model.Doctor.FirstName;
                doctor.LastName = model.Doctor.LastName;
                doctor.FatherName = model.Doctor.FatherName;
                clinic.ClinicName = model.Clinic.ClinicName;
                clinic.ClinicEmail = model.Clinic.ClinicEmail;
                clinic.ClinicPhone = model.Clinic.ClinicPhone;
                db.SaveChanges();
            }

            return RedirectToAction("Management");
        }

        //GET: Clinics/Symptoms
        public ActionResult Symptoms(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointment = db.Appointments.Where(a => a.ClinicId == clinicId)
                .Select(Mapper.Map<Appointment, AppointmentDTO>)
                .FirstOrDefault(a => a.AppointmentId == id.Value);

            if (appointment == null)
                return HttpNotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Symptoms(int? id, AppointmentDTO appointment)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            if (!ModelState.IsValid && string.IsNullOrWhiteSpace(appointment.Symptoms))
            {
                return RedirectToAction("Symptoms");
            }

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointmentInDb = db.Appointments
                .Where(a => a.ClinicId == clinicId)
                .FirstOrDefault(a => a.AppointmentId == id.Value);

            if (appointmentInDb == null)
                return HttpNotFound();

            appointmentInDb.Symptoms = appointment.Symptoms;
            db.SaveChanges();

            return RedirectToAction("AppointmentDetails", new { id = appointmentInDb.AppointmentId });
        }

        //GET: Clinics/Symptoms
        public ActionResult Remarks(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointment = db.Appointments.Where(a => a.ClinicId == clinicId).Select(Mapper.Map<Appointment, AppointmentDTO>).FirstOrDefault(a => a.AppointmentId == id.Value);
            if (appointment == null)
                return HttpNotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remarks(int? id, AppointmentDTO appointment)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            if (!ModelState.IsValid && string.IsNullOrWhiteSpace(appointment.Symptoms))
            {
                return RedirectToAction("Remarks");
            }
            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            var appointmentInDb = db.Appointments.Where(a => a.ClinicId == clinicId).FirstOrDefault(a => a.AppointmentId == id.Value);
            if (appointmentInDb == null)
                return HttpNotFound();

            appointmentInDb.Remarks = appointment.Remarks;
            db.SaveChanges();

            return RedirectToAction("AppointmentDetails", new { id = appointmentInDb.AppointmentId });
        }

        //GET: Clinics/EditClinicProfile
        public ActionResult AddCertificate()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCertificate(CertificateDTO certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid && string.IsNullOrWhiteSpace(certificate.CertifcateDescription))
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            int doctorId = GetUserId();
            var certificteToDb = Mapper.Map<CertificateDTO, Certifcate>(certificate);
            certificteToDb.UserId = doctorId;
            db.Certifcates.Add(certificteToDb);
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        //GET: Clinics/EditClinicProfile
        public ActionResult AddMedicine()
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedicine(CertificateDTO certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid && string.IsNullOrWhiteSpace(certificate.CertifcateDescription))
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            return RedirectToAction("Management");
        }

        //GET: Clinics/EditClinicProfile/{appointmentId}
        public ActionResult AddPrescription(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            FormPrescritpionDTO model = new FormPrescritpionDTO { AppointmentId = id.Value };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrescription(int? id, FormPrescritpionDTO prescription)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            if (!id.HasValue)
                return RedirectToAction("AppointmentDetails");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            var prescritionToDb = Mapper.Map<FormPrescritpionDTO, Prescription>(prescription);
            prescritionToDb.AppointmentId = id.Value;
            db.Prescriptions.Add(prescritionToDb);
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        //Get: Clinics/EditCertification/{CertificationId}
        public ActionResult EditCertificate(int? id)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!id.HasValue)
                return RedirectToAction("Management");

            var certificate = db.Certifcates.Select(Mapper.Map<Certifcate, CertificateDTO>).FirstOrDefault(c => c.CertifcateID == id);
            if (certificate == null)
                return HttpNotFound();

            return View(certificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCertificate(int? id, CertificateDTO certificate)
        {
            if (SessionIsNull())
                return RedirectToAction("SignIn", "Accounts");

            int? clinicId = GetClinicId();
            if (!clinicId.HasValue)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not valid");
                return View();
            }

            if (!id.HasValue)
                return RedirectToAction("Management");

            var certificateInDb = db.Certifcates.FirstOrDefault(c => c.CertifcateID == id);
            if (certificateInDb == null)
                return HttpNotFound();

            int doctorId = GetUserId();
            certificateInDb = Mapper.Map<CertificateDTO, Certifcate>(certificate);
            certificateInDb.UserId = doctorId;
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        public int? GetClinicId()
        {
            if (Session["ClinicId"] != null)
                return int.Parse(Session["ClinicId"].ToString());
            return null;
        }

        public bool SessionIsNull() => Session["UserId"] == null;
        public int GetUserId() => int.Parse(Session["UserId"].ToString());
    }
}