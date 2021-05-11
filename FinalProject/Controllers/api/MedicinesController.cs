using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FinalProject.Controllers.api
{
    public class MedicinesController : ApiController
    {
        private MyAppContext db = new MyAppContext();

        public IHttpActionResult Get()
        {
            return Ok(db.Medicines.ToList());
        }

        public IHttpActionResult Get(int id)
        {
            var medicine = db.Medicines.Find(id);
            if (medicine == null)
                return NotFound();

            return Ok(medicine);
        }

        public IHttpActionResult Create([Bind(Exclude ="MedicineId")] Medicine medicine)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Medicine");

            var medicinetocreate = new Medicine();

            try
            {
                db.Medicines.Add(medicinetocreate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created(Request.RequestUri, medicine);
        }
    }
}
