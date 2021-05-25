using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.ViewModels;
using System;
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
                ModelState.AddModelError("", "");
                return View();
            }

            var schedule = Mapper.Map<ScheduleDTO, Schedule>(day);
            schedule.ClinicId = Convert.ToInt32(Session["ClinicId"]);
            db.Schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        public ActionResult Schedule()
        {
            return View();
        }

        public ActionResult AppointmentDetails()
        {
            return View();
        }

        //public ActionResult AppointmentDetails()
        //{
        //    return View();
        //}

        public ActionResult Location()
        {
            return View();
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