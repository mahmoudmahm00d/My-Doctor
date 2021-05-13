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
    public class PharmaciesController : Controller
    {
        private MyAppContext db = new MyAppContext();

        // GET: Pharmacies
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PharmacyDTO Pharmacy, HttpPostedFileBase certifacte)
        {
            if (!ModelState.IsValid || certifacte == null || certifacte.ContentLength == 0)
                return View();

            int userId = db.Pharmacies.Select(c => c.UserId == Pharmacy.ForUser.UserId).Count();

            if (userId > 1)
                return View();

            var PharmacyInDB = Mapper.Map<PharmacyDTO, Pharmacy>(Pharmacy);
            string path = Path.Combine(Server.MapPath("~/Certificates"), certifacte.FileName);
            PharmacyInDB.Certificate = path;
            db.Pharmacies.Add(PharmacyInDB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}