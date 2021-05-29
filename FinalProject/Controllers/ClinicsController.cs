using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClinicDTO clinic, HttpPostedFileBase certifacte)
        {
            //if(SessionIsNull())
            //    return RedirectToAction("SignIn", "Accounts");

            if (!ModelState.IsValid || certifacte == null || certifacte.ContentLength == 0)
                return View();

            int userId = db.Clinics.Select(c => c.UserId == clinic.ForUser.UserId).Count();

            if (userId > 1)
                return View();

            var clinicInDB = Mapper.Map<ClinicDTO, Clinic>(clinic);
            string path = Path.Combine(Server.MapPath("~/Certificates"), certifacte.FileName);
            certifacte.SaveAs(path);
            clinicInDB.Certificate = path;
            clinicInDB.IsActiveClinic = false;
            db.Clinics.Add(clinicInDB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddDay()
        {
            AddDayViewModel model = new AddDayViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddDay(ScheduleDTO day)
        {
            //todo
            //Add Session
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Schedule");
                return View();
            }
            if (day.FromTime > day.ToTime)
            {
                ModelState.AddModelError("", "invalid schedule");
                return View();
            }
            var check = db.Schedules.FirstOrDefault(s => s.FromTime.Hour <= day.FromTime.Hour && s.FromTime.Hour <= s.ToTime.Hour);
            if (check != null)
            {
                ModelState.AddModelError("", "invalid schedule");
                return View();
            }

            var schedule = Mapper.Map<ScheduleDTO, Schedule>(day);
            schedule.ClinicId = Convert.ToInt32(Session["ClinicId"]);
            db.Schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        public ActionResult Appointments()
        {
            return View();
        }

        public ActionResult Schedule()
        {
            return View();
        }

        public ActionResult AppointmentDetails()
        {
            return View();
        }

        public ActionResult Location()
        {
            

            var cities = db.Cities.Where(c => c.IsActiveCity == true).Select(Mapper.Map<City,CityDTO>);
            LocationViewModel model = new LocationViewModel {Cities = cities }; 
            return View(model);
        }

        [HttpPost]
        public ActionResult Location(LocationDTO location)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Location");
                return View();
            }
            var locationToDb = Mapper.Map<LocationDTO, Location>(location);
            locationToDb.ClinicId = Convert.ToInt32(Session["ClinicId"]);
            db.Locations.Add(locationToDb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditClinicProfile()
        {
            return View();
        }

        public ActionResult EditPassword()
        {
            return View();
        }

        public ActionResult EditEmail()
        {
            return View();
        }

        public bool SessionIsNull() => Session["UserId"] == null;
    }
}