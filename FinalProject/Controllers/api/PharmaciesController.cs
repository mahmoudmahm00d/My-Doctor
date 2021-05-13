using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.ViewModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FinalProject.Controllers.api
{
    public class PharmaciesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(db.Pharmacies.Include(c => c.ForUser).ToList().Select(Mapper.Map<Pharmacy, PharmacyDTO>));
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var clinic = db.Pharmacies.Include(c => c.ForUser).FirstOrDefault(c => c.PharmacyId == id);

            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Pharmacy, PharmacyDTO>(clinic));
        }

        [HttpPost]
        public IHttpActionResult Create(PharmacyDTO pharmacy)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Clinic Properties");

            int userId = db.Pharmacies.Select(c => c.UserId == pharmacy.ForUser.UserId).Count();

            if (userId > 1)
                return BadRequest();

            db.Pharmacies.Add(Mapper.Map<PharmacyDTO, Pharmacy>(pharmacy));
            return Created(Request.RequestUri.ToString(), pharmacy);
        }

        [HttpPut]
        public IHttpActionResult Edit(int id, PharmacyOnlyDTO pharmacy)
        {
            if (!ModelState.IsValid || pharmacy.PharmacyId != id)
                return BadRequest("Invalid Clinic Properties");

            var pharmacyInDb = db.Pharmacies.Find(id);

            if (pharmacyInDb == null)
                return NotFound();

            Mapper.Map(pharmacy, pharmacyInDb);
            db.SaveChanges();
            return Ok(pharmacy);
        }
    }
}
