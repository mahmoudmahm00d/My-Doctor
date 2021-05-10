using Graduate.DTOs;
using Graduate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Graduate.Controllers.api
{
    public class ClinicsController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        public IHttpActionResult Get()
        {
            return Ok(db.Clinics.ToList());
        }

        public IHttpActionResult Get(int id)
        {
            var clinic = db.Clinics.FirstOrDefault(c => c.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }
            return Ok(clinic);
        }

        [HttpPost]
        public IHttpActionResult Create(ClinicDTO clinic)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Clinic Properties");

            //ToDo
            var clinicdb = new Clinic();
            db.Clinics.Add(clinicdb);
            return Created(new Uri(Request.RequestUri.ToString()), clinic);
        }

        [HttpPut]
        public IHttpActionResult Edit(int id, ClinicDTO clinic)
        {
            var clinicToUpdate = db.Clinics.Find(id);

            if (clinicToUpdate == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest("Invalid Clinic Properties");


            //ToDo
            var clinicdb = new Clinic();
            db.Clinics.Add(clinicdb);
            return Created(new Uri(Request.RequestUri.ToString()), clinic);
        }
    }
}
