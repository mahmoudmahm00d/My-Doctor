using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using System;
using System.Collections.Generic;
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClinicDTO clinic, HttpPostedFileBase certifacte)
        {
            if (!ModelState.IsValid || certifacte == null || certifacte.ContentLength == 0)
                return View();

            int userId = db.Clinics.Select(c => c.UserId == clinic.ForUser.UserId).Count();

            if (userId > 1)
                return View();

            var clinicInDB = Mapper.Map<ClinicDTO, Clinic>(clinic);
            string path = Path.Combine(Server.MapPath("~/Certificates"), certifacte.FileName);
            clinicInDB.Certificate = path;
            db.Clinics.Add(clinicInDB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}